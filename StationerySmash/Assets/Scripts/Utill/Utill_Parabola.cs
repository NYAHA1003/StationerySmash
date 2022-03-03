using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public class Utill_Parabola : MonoBehaviour
    {
        /// <summary>
        /// 최고점 계산
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <returns></returns>
        static public float Caculated_Height(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin) * Mathf.Sin(sin)) / Mathf.Abs((Physics2D.gravity.y * 2));
        }

        /// <summary>
        /// 수평 도달 거리 계산
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안 값</param>
        /// <returns></returns>
        static public float Caculated_Width(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin * 2)) / Mathf.Abs(Physics2D.gravity.y);
        }

        /// <summary>
        /// 최고점 도달 시간
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="multiple">시간 배수. 1이면 최고점 도달 시간, 2이면 수평 도달 시간</param>
        /// <returns></returns>
        static public float Caculated_Time(float v, float sin, float multiple)
        {
            return (v * Mathf.Sin(sin) / Mathf.Abs(Physics2D.gravity.y)) * multiple;
        }

        /// <summary>
        /// t초 후의 위치
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="time">시간</param>
        /// <returns></returns>
        static public float Caculated_TimeToPos(float v, float sin, float time)
        {
            return (v * time * Mathf.Sin(sin)) - (Mathf.Abs(Physics2D.gravity.y / 2) * (time * time));
        }
    }

}