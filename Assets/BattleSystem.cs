using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Status do jogo
public enum BattleState { 
	START, 
	PLAYER_TURN, 
	ENEMY_TURN, 
	WON, 
	LOST 
}

public class BattleSystem : MonoBehaviour
{
	// Variáveis 
	[Header("Configuração de combate")]
	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	// Estanciamento do objeto
	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;
	
	// Diálogo
	public Text dialogueText;

	[Header("Balanceamento dos campeão")]
	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
		// Status do começo do jogo
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

    //Sistema de combate geral
    IEnumerator SetupBattle(){
            //Var provisório playerGO/enemyGO para guardar os valores depois usar get
            GameObject playerGO = Instantiate(playerPrefab);
            playerUnit = playerGO.GetComponent<Unit>();

            GameObject enemyGO = Instantiate(enemyPrefab);
            enemyUnit = enemyGO.GetComponent<Unit>();

            //Diálago entre campeãoes
            dialogueText.text = "O inimigo " + enemyUnit.unitName + " está pronto para batalhar com você ate a morte...";

            playerHUD.SetHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYER_TURN;
            PlayerTurn();
	}

	// Sistema de Atacar o oponete
	IEnumerator PlayerAttack(){
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Você atacou a " + enemyUnit.unitName;

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
				
        }
        else
        {
            state = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
        
	}

	// sistem quando o player é ferido em batalha
	IEnumerator EnemyTurn(){
		dialogueText.text = enemyUnit.unitName + " feriu você em batalha!";

		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead){
			state = BattleState.LOST;
			EndBattle();
		} else{	
			state = BattleState.PLAYER_TURN;
			PlayerTurn();
		}
	}

	// Sistem sé você venceu o perdeu o combate até a morte
	void EndBattle(){
		if(state == BattleState.WON){
			dialogueText.text = "Você sobreviveu para ver Sol narcer amanhã, " + playerUnit.unitName;
		} else if (state == BattleState.LOST){
			dialogueText.text = "Você foi derrotado em combate.";
		}
	}

	// Começo do combate
	void PlayerTurn(){
		dialogueText.text = "Escolha uma opção rapido...";
	}

	IEnumerator PlayerHeal(){
		playerUnit.Heal(5);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "Você se curou!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMY_TURN;
		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton(){
		if (state != BattleState.PLAYER_TURN )
			return;

		StartCoroutine(PlayerAttack());
	}

	public void OnHealButton(){
		if (state != BattleState.PLAYER_TURN)
			return;

		StartCoroutine(PlayerHeal());
	}
	// Caio Vilarouca
}
