using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //public Rigidbody rd;    //public����private���ӿڣ�
    //public int score = 0;   //������ֵ
    public Text scoreText;  //�������UI
    //public GameObject winText;  //��ʤ����UI��λΪ��Ϸ���壨Ĭ�ϲ���ʾ����������ʾ��
    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = PublicTool.GetGameData();
        //Debug.Log("��Ϸ��ʼ�ˣ�");
        //rd = GetComponent<Rigidbody>(); // ���ø������

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Crystal��" + gameData.GetNumActiveCrystal();
        //Debug.Log("��Ϸ��������!")��
        //rd.AddForce(Vector3.right); //ʩ��1N��vector3.right left forward back��
        //rd.AddForce(new Vector3(10, 0, 0));  //�Զ�����

        //float h = Input.GetAxis("Horizontal");  //keyboard A/D~~~-1/1
        //float v = Input.GetAxis("Vertical");    //keyboard W/S~~~-1/1
        //Debug.Log(h); (1,2,3) * 2 = (2,4,6)   //����
        //rd.AddForce(new Vector3(h, 0, v));  //x y z
    }
}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("������ײ��");

    //    //��Foodģ�����úñ�ǩ����⵽�����Ӧ��ǩ������
    //    if (collision.gameObject.tag == "Food")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}

    // ����ע��ctrl+k ctrl+c ȡ��ע��ctrl+k ctrl+u
    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("������ײ��");
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    //Debug.Log("������ײ��");
    //}

    /*private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("��������" + other.tag);
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);

            score++;    //��һ��Food����+1
            scoreText.text = "������" + score;

            //�ж���Ϸʤ��
            if (score == 8)
            {
                winText.SetActive(true);    //����UI
            }
        }
    }*/

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("�����˳�" + other.tag);
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("��������" + other.tag);
    //}
//}





