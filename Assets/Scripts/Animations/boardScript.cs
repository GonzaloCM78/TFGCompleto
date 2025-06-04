using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardScript : MonoBehaviour
{

    public int boards, previousBoards;
    public Animator[] boardAnim;

    
    void Start()
    {
        boardAnim = GetComponentsInChildren<Animator>();
    }

    
    void Update()
    {
        if(boards != previousBoards)
        {
            previousBoards = boards;

            switch(boards)
            {
                case 1:
                    boardAnim[0].Play("boardAnimation1");
                    return;
                case 2:
                    boardAnim[1].Play("boardAnimation2");
                    return;
                case 3:
                    boardAnim[2].Play("boardAnimation3");
                    return;
                case 4:
                    boardAnim[3].Play("boardAnimation4");
                    return;
                case 5:
                    boardAnim[4].Play("boardAnimation5");
                    return;
                case 6:
                    boardAnim[5].Play("boardAnimation6");
                    return;

            }
        }
    }
}
