using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

	Rigidbody2D rigid2D;
	Animator animator;
	float jumpForce = 680.0f;
	float walkForce = 20.0f;
	float maxWalkSpeed = 2.0f;

	GameMgr m_GameMgr = null;

	public GameObject m_BulletObj = null;
	GameObject m_CollSvObj = null; // 보상을 두번 주는 걸 막는 부분

	[HideInInspector] public static int m_LifeCount = 3;
	public RawImage[] m_HeartImg;
	float m_DamageDelay = 1.5f;

	void Start() {
		this.rigid2D = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();

		m_GameMgr = FindObjectOfType<GameMgr>(); // 스크립트가 여러군데 붙어있으면 사용 불가
		// = GameObject a_GameMgr = GameObject.Find("GameMgr");
		// if(a_GameMgr != null)
		//     m_GameMgr = a_GameMgr.GetComponent<GameMgr>();
	}

	// Ctrl + ] 중괄호 짝 찾기
	void Update() {

		if (0.0f < m_DamageDelay)
			m_DamageDelay = m_DamageDelay - Time.deltaTime;

		// 점프한다
		if(Input.GetKeyDown(KeyCode.UpArrow) && this.rigid2D.velocity.y == 0) {
			this.rigid2D.AddForce(transform.up * this.jumpForce);
		}

		// 좌우이동
		int key = 0;
		if(Input.GetKey(KeyCode.RightArrow)) key = 1;
		if(Input.GetKey(KeyCode.LeftArrow)) key = -1;

		// 플레이어 속도
		float speedx = Mathf.Abs(this.rigid2D.velocity.x);

		// 스피드 제한
		if(speedx < this.maxWalkSpeed) {
			this.rigid2D.AddForce(transform.right * key * this.walkForce);
		}

		// 반전대책
		if(key != 0) {
			transform.localScale = new Vector3(key, 1, 1);
		}

		// 플레이어 속도에 맞춰서 애니메이션 속도를 바꾼다
		this.animator.speed = speedx / 2.0f;
	

		// 화면 밖으로 나간 경우는 처음부터
		if(transform.position.y < -10) 
		{
			TakeDamage();

			if(0 < m_LifeCount)
			SceneManager.LoadScene("GameScene");
		}

		if(Input.GetKeyDown(KeyCode.Space))
        {
			GameObject newObj = (GameObject)Instantiate(m_BulletObj);
			Vector3 a_CacDir = new Vector3(transform.localScale.x, 0.0f, 0.0f); // localscale 바라보는방향
			BulletCtrl a_BulletSC = newObj.GetComponent<BulletCtrl>();
			a_BulletSC.BulletSpawn(this.transform, a_CacDir);
        }

		for(int i = 0; i < m_HeartImg.Length; i++)
        {
			if (i < m_LifeCount)
				m_HeartImg[i].gameObject.SetActive(true);

			else
				m_HeartImg[i].gameObject.SetActive(false);
        }
	}

	// 골 도착
	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.name.Contains("Apple") == true)
        {
			if(m_CollSvObj != other.gameObject)
            {
				m_GameMgr.GetItem(1);
				m_CollSvObj = other.gameObject;
            }

			Destroy(other.gameObject);
        }

		else if (other.gameObject.name.Contains("Banana") == true)
        {
			if(m_CollSvObj != other.gameObject)
            {
				m_GameMgr.GetItem(2);
				m_CollSvObj = other.gameObject;
            }

			Destroy(other.gameObject);
        }

		else if (other.gameObject.name.Contains("flag") == true)
		{
			Debug.Log("골");
			SceneManager.LoadScene("ClearScene");
		}

		else if(other.gameObject.name.Contains("Zombi") == true)
        {
			TakeDamage();

			Vector3 pushDir = this.transform.position - other.transform.position;
			pushDir.y += 1.4f;
			pushDir.Normalize();
			this.rigid2D.AddForce(pushDir * 500.0f);

        }
	}

	public void TakeDamage()
    {
		if (0.0f < m_DamageDelay)
			return;

		m_DamageDelay = 1.5f;
		m_LifeCount--;
		m_GameMgr.ReSetItem();

		if(m_LifeCount <= 0)
        {
			m_LifeCount = 0;
			SceneManager.LoadScene("ClearScene");
        }
    }
}