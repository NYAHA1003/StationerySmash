using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public class Utill_Parabola : MonoBehaviour
    {
        /// <summary>
        /// �ְ��� ���
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���Ȱ�</param>
        /// <returns></returns>
        static public float Caculated_Height(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin) * Mathf.Sin(sin)) / Mathf.Abs((Physics2D.gravity.y * 2));
        }

        /// <summary>
        /// ���� ���� �Ÿ� ���
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���� ��</param>
        /// <returns></returns>
        static public float Caculated_Width(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin * 2)) / Mathf.Abs(Physics2D.gravity.y);
        }

        /// <summary>
        /// �ְ��� ���� �ð�
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���Ȱ�</param>
        /// <param name="multiple">�ð� ���. 1�̸� �ְ��� ���� �ð�, 2�̸� ���� ���� �ð�</param>
        /// <returns></returns>
        static public float Caculated_Time(float v, float sin, float multiple)
        {
            return (v * Mathf.Sin(sin) / Mathf.Abs(Physics2D.gravity.y)) * multiple;
        }

        /// <summary>
        /// t�� ���� ��ġ
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���Ȱ�</param>
        /// <param name="time">�ð�</param>
        /// <returns></returns>
        static public float Caculated_TimeToPos(float v, float sin, float time)
        {
            return (v * time * Mathf.Sin(sin)) - (Mathf.Abs(Physics2D.gravity.y / 2) * (time * time));
        }
    }

}