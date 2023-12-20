using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerswapper : MonoBehaviour
{
    [SerializeField]
    RuntimeAnimatorController controller1;
    [SerializeField]
    RuntimeAnimatorController controller2;
    [SerializeField]
    Animator animator;
    int activeController;
    // Start is called before the first frame update
    void Start()
    {
        activeController = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeControllers();
        }
    }

    void ChangeControllers()
    {
        switch (activeController)
        {
            case 1:
                float prevNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                AnimationClip a = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                AnimatorOverrideController animController = new AnimatorOverrideController(controller2);
                animController.name = controller2.name;
                animController["none"] = a;
                animator.runtimeAnimatorController = animController;
                animator.Play("dummyState", 0, prevNormalizedTime);
                StartCoroutine(WaitOneFrameThenCrossFade("california"));


                Debug.Log($"switching to controller2 should be a smooth transition");
                activeController = 2;
                break;
            case 2:
                animator.runtimeAnimatorController = controller1;
                Debug.Log($"switching to controller1 should be a crunchy transition");
                activeController = 1;
                break;
        }
    }
    IEnumerator WaitOneFrameThenCrossFade(string animState)
    {
        yield return 0;
        animator.CrossFadeInFixedTime(animState, 1f, 0); //for some reason doing this on the same frame as Animator.Play cause the wrong state to play???
                                                         //I don't really know why this is the case but I'm not super well versed with Unity's systems
    }
}
