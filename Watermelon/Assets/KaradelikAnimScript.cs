using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class KaradelikAnimScript : MonoBehaviour
{
    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;
    private int m_IndexSprite;

    bool IsDone;
    public GameObject KaradelikButonObj;

    private void Start()
    {
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI());
    }


    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsDone == false)
            StartCoroutine(Func_PlayAnimUI());
    }
}
