using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePageBase : MonoBehaviour {

	List<CharactorStatus> friendList;
	List<CharactorStatus> enemyList;

	GameObject canvas;
	GameObject diceObject = null; //いらないかも
	GameObject diceText = null;
	int diceNum = 0;
	int sumDiceNum = 0;

	//TODO test value 後に削除 --START--
	int TEST_FRIEND_NUM = 4;
	int TEST_ENEMY_NUM = 4;
	int TEST_HP = 100;
	int TEST_ATTACK = 10;
	string TEST_FIEND_NAME = "味方";
	string TEST_ENEMY_NAME = "敵";
	int TEST_CHARA_BASE_POS_Y = 450;
	//TODO test value 後に削除 ---END---

	int phaseFlg = 0;
	int DICE_PHASE = 1;
	int BATTLE_PHASE = 2;



	// Use this for initialization
	void Start () {
		CreatePage ();
		phaseFlg = 1;
	}

	void Update() {

		//switchでいい
		if (phaseFlg == DICE_PHASE) {
			RollDice ();
		} else if (phaseFlg == BATTLE_PHASE) {
			Battle();
		}
	}

	void CreatePage() {
		// TODO キャラクター生成・配置
		//



		// TODO test値設定	------------------START----------------- //
		// 敵・味方で処理統一したほうがいい
		friendList = new List<CharactorStatus>();
		enemyList = new List<CharactorStatus>();

		canvas = GameObject.Find("Canvas");

		// 味方ステータス設定
		for (int i = 0; i < TEST_FRIEND_NUM; i++) {
			CharactorStatus status = new CharactorStatus ();
			status.hp = status.maxHp = TEST_HP;
			status.attack = TEST_ATTACK;
			status.name = TEST_FIEND_NAME + i.ToString ();
			friendList.Add (status);

			// 画像等配置
			GameObject friendObject = new GameObject ();
			friendObject.AddComponent<RectTransform> ();
			friendObject.AddComponent<Image> ();
			friendObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (150, 150);
			friendObject.GetComponent<RectTransform> ().position = new Vector2 (-250 + (i % 2) * 100, TEST_CHARA_BASE_POS_Y - i * 250);
			friendObject.transform.SetParent(canvas.transform, false);
			friendObject.name = "FriendObject_" + i.ToString ();

			// テキスト周り配置
			GameObject friendHpText = new GameObject ("HpText");
			friendHpText.AddComponent<Text> ();
			friendHpText.GetComponent<Text> ().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			friendHpText.GetComponent<Text> ().text = status.hp.ToString () + "/" + status.maxHp.ToString ();
			friendHpText.GetComponent<Text> ().fontSize = 30;
			friendHpText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			friendHpText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 50);
			friendHpText.transform.position = new Vector2 (0, 90);
			friendHpText.transform.SetParent(friendObject.transform, false);

			GameObject friendNameText = new GameObject ("nameText");
			friendNameText.AddComponent<Text> ();
			friendNameText.GetComponent<Text> ().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			friendNameText.GetComponent<Text> ().text = status.name;
			friendNameText.GetComponent<Text> ().fontSize = 30;
			friendNameText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			friendNameText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 50);
			friendNameText.transform.position = new Vector2 (0, -90);
			friendNameText.transform.SetParent(friendObject.transform, false);
		}

		// 敵ステータス設定
		for (int i = 0; i < TEST_ENEMY_NUM; i++) {
			CharactorStatus status = new CharactorStatus ();
			status.hp = status.maxHp = TEST_HP;
			status.attack = TEST_ATTACK;
			status.name = TEST_ENEMY_NAME + i.ToString ();
			enemyList.Add (status);
			// 画像配置
			GameObject enemyObject = new GameObject ();
			enemyObject.AddComponent<RectTransform> ();
			enemyObject.AddComponent<Image> ();
			enemyObject.GetComponent<Image> ().color = new Color(0,0,0);
			enemyObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (150, 150);
			enemyObject.GetComponent<RectTransform> ().position = new Vector2 (250 - (i % 2) * 100, TEST_CHARA_BASE_POS_Y - i * 250);
			enemyObject.transform.SetParent(canvas.transform, false);
			enemyObject.name = "EnemyObject_" + i.ToString ();
			// テキスト周り配置
			GameObject enemyHpText = new GameObject ("HpText");
			enemyHpText.AddComponent<Text> ();
			enemyHpText.GetComponent<Text> ().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			enemyHpText.GetComponent<Text> ().text = status.hp.ToString () + "/" + status.maxHp.ToString ();
			enemyHpText.GetComponent<Text> ().fontSize = 30;
			enemyHpText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			enemyHpText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 50);
			enemyHpText.transform.position = new Vector2 (0, 90);
			enemyHpText.transform.SetParent(enemyObject.transform, false);

			GameObject enemyNameText = new GameObject ("nameText");
			enemyNameText.AddComponent<Text> ();
			enemyNameText.GetComponent<Text> ().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			enemyNameText.GetComponent<Text> ().text = status.name;
			enemyNameText.GetComponent<Text> ().fontSize = 30;
			enemyNameText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			enemyNameText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 50);
			enemyNameText.transform.position = new Vector2 (0, -90);
			enemyNameText.transform.SetParent(enemyObject.transform, false);
		}

		//スキルポイント合計値

		// TODO test値設定	-------------------END------------------ //
	}

	void RollDice() {
		// サイコロが生成されてなかったら生成
		if (diceObject == null) {
			diceObject = new GameObject ();
			diceObject.transform.SetParent(canvas.transform, false);

			GameObject panel = new GameObject ("DicePanel");
			panel.AddComponent<Image> ();
			panel.GetComponent<Image> ().color = new Color (0.3f, 0.3f, 0.3f, 0.3f);
			panel.GetComponent<RectTransform> ().sizeDelta = new Vector2 (600, 1300);
			panel.transform.SetParent(diceObject.transform, false);

			diceText = new GameObject ("diceText");
			diceText.AddComponent<Text> ();
			diceText.GetComponent<Text> ().font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
			diceText.GetComponent<Text> ().text = Random.Range (0, 8).ToString ();
			diceText.GetComponent<Text> ().fontSize = 60;
			diceText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (150, 150);
			diceText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			diceText.transform.SetParent(diceObject.transform, false);
		} else if(diceNum == 0){
			int num = Random.Range (0, 8);
			diceText.GetComponent<Text> ().text = num.ToString ();

			// tapされたらダイス確定
			if (Input.GetMouseButton (0)) {
				diceNum = num;
				sumDiceNum += num;
				GameObject.Find ("Canvas/SumSkillPointText").GetComponent<Text>().text = sumDiceNum.ToString();
				Destroy (diceObject);
				diceObject = null;
				diceText = null;
				diceNum = 0;
				// battleフェイズに移行
				phaseFlg = 2;
			}
		}
	}

	void Battle() {

	}

	// 攻撃ボタンが押された時
	public void pressAttack() {
		phaseFlg = 1;

		// TODO test 敵のHP減少
		//GameObject.Find ("Canvas/EnemyObject_0/hpText").GetComponent<Text>().text = "90/100";

	}

	public void setPhaseFlg(int i) {
		phaseFlg = i;
	}

}
