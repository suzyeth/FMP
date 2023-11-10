using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //public Rigidbody rd;    //public或者private（接口）
    //public int score = 0;   //分数初值
    public Text scoreText;  //定义分数UI
    //public GameObject winText;  //将胜利的UI定位为游戏物体（默认不显示，结束后显示）
    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = PublicTool.GetGameData();
        //Debug.Log("游戏开始了！");
        //rd = GetComponent<Rigidbody>(); // 调用刚体组件

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Crystal：" + gameData.GetNumActiveCrystal();
        //Debug.Log("游戏正在运行!")；
        //rd.AddForce(Vector3.right); //施加1N（vector3.right left forward back）
        //rd.AddForce(new Vector3(10, 0, 0));  //自定义力

        //float h = Input.GetAxis("Horizontal");  //keyboard A/D~~~-1/1
        //float v = Input.GetAxis("Vertical");    //keyboard W/S~~~-1/1
        //Debug.Log(h); (1,2,3) * 2 = (2,4,6)   //加速
        //rd.AddForce(new Vector3(h, 0, v));  //x y z
    }
}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("发生碰撞了");

    //    //给Food模板设置好标签，检测到物体对应标签就销毁
    //    if (collision.gameObject.tag == "Food")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}

    // 多行注释ctrl+k ctrl+c 取消注释ctrl+k ctrl+u
    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("结束碰撞了");
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    //Debug.Log("保持碰撞了");
    //}

    /*private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("触发进入" + other.tag);
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);

            score++;    //吃一个Food分数+1
            scoreText.text = "分数：" + score;

            //判断游戏胜利
            if (score == 8)
            {
                winText.SetActive(true);    //激活UI
            }
        }
    }*/

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("触发退出" + other.tag);
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("触发保持" + other.tag);
    //}
//}





