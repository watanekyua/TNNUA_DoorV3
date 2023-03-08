using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using DG.Tweening;
using UnityEngine.Playables;

public class BuddhaMove : BasePresent
{
    Tweener twing;
    public override void StartPresent(){
        gameObject.SetActive(true);
        playable.Stop();
        playable.Play();
        if(!this.enabled)
            return;
        StartCoroutine(IStartMove());
    }

    IEnumerator IStartMove()
    {
        if(twing != null){
            twing.Kill();
            twing = null;
        }

        transform.position = orinPos;

        yield return new WaitForSeconds(delayMove);

        twing = transform.DOLocalMove(fpos, speed);
    }

    public override void StopPresent(){
        playable.Stop();

        gameObject.SetActive(false);
    }
}
