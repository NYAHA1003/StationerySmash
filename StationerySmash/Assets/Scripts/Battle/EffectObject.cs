using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSys;
    [SerializeField]
    private float delete_time = 0.5f;

    public void Set_Effect(Vector2 pos)
    {
        CancelInvoke();

        //ÀÌÆåÆ® ¸®¼Â
        gameObject.SetActive(true);
        particleSys.Stop();
        particleSys.Play();

        transform.position = pos;
        
        Invoke("Delete_Effect", delete_time);
    }

    private void Delete_Effect()
    {
        gameObject.SetActive(false);
    }
}
