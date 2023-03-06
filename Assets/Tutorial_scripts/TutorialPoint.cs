using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialPoint : MonoBehaviour
{
    private EventSystem eventSystem; // Reference to the EventSystem component
    private GameObject controlsUI;
    private GameObject player;
    private GameObject mathInfo;
    private GameObject mathManager;

    public TextMeshProUGUI instruction;

    private float startTime;

    //common var for freezing screen
    private GameObject freezeLikeImg;
    public PlayerCamera playerCamera;

    //Welcome msg and WASD Tutorial vars
    private bool WelcomeMsgDone = false;
    private GameObject clickImg;
    private bool wasdCheck = false;
    private Vector3 startPosition;  //to Check how further they made move;
    private int welcomIndex = 0;
    string[] welcomeMsgs =
    {
        "Welcome to Swinging Game!",
        "Let's learn how to move!",
        "Use WASD to move",
        "Try yourself!",
    };

    //Mouse move checker
    private bool mouseCheckDone = true;
    private Vector3 currPosition;   //to make player cant move on mouse section;


    //jump checker
    private bool jumpCheckDone = true;
    private int jumpIndex = 0;
    string[] jumpMsgs =
    {
        "Let's learn how to Jump",
        "Look higher than where you want to jump",
        "Press SPACE BAR",
        "Try yourself",
    };


    //swing checker
    private bool swingCheckerDone = true;
    private int swingIndex = 0;
    string[] swingMsgs =
    {
        "Let's learn how to SWING",
        "Aim your point to SHPERE",
        "Click and hold the left click",
        "Then swing with WASD Key",
        "Release after you made enough momentum ",
        "Try yourself",
    };

    //it is number and calculate number
    public int worldSum = 0;
    private int bossNumber = 13;
    private bool coinCheckDone = true;
    private int coinIndex = 0;
    private GameObject bossNumberText;
    string[] coinMsgs =
    {
        "Final mission of Tutorial 1",
        "Make the larger number than BOSSnumber",
        "Pick up number you want",
        "Be careful the LASER WALL!",
        "Go and get numbers!",
    };


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCapsule");
        player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;

        freezeLikeImg = clickImg = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("ScreenFreeze").gameObject;
        clickImg = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("Left_Click").gameObject;
        bossNumberText = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("BossNumber").gameObject;
        //luanchBar = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("LaunchBar").gameObject; Do not require anymore
        //clickImg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!WelcomeMsgDone)
        {
            WelcomMsg();
        }

        if (!mouseCheckDone)
        {
            MouseMoveChecker();
        }

        if (!jumpCheckDone)
        {
            JumpChecker();
        }

        if (!swingCheckerDone)
        {
            SwingChecker();
        }

        if (!coinCheckDone)
        {
            CoinChecker();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "MouseTutorial")
        {
            //Debug.Log("trigger mouse");

            mouseCheckDone = false;   //triger mouseCheck function;

            //freezing screen
            freezeLikeImg.SetActive(true);

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;
            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            DontJumpOut();

            startTime = Time.time;
        }

        if (gameObject.name == "JumpTutorial")
        {
            jumpCheckDone = false;
            freezeLikeImg.SetActive(true);

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;

            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            DontJumpOut();
            
        }

        if (gameObject.name == "SwingTutorial")
        {
            swingCheckerDone = false;
            freezeLikeImg.SetActive(true);

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;
            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            DontJumpOut();
        }

        if (gameObject.name == "CoinTutorial")
        {
            coinCheckDone = false;
            freezeLikeImg.SetActive(true);

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;
            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            DontJumpOut();
        }

        if (gameObject.name == "ModularText")
        {
            int num = int.Parse(gameObject.transform.Find("3D Text Prefab").GetComponent<TextScript>().EnterTextHere);
            DoMath(num);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.name == "MouseTutorial")
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        if (gameObject.name == "CoinTutorial")
        {

            instruction.text = "";
        }

        if (gameObject.name == "JumpTutorial")
        {
            StartCoroutine(SuccessMsg());
            GetComponent<BoxCollider>().enabled = false;
            jumpIndex++;
        }

        if (gameObject.name == "SwingTutorial")
        {
            StartCoroutine(SuccessMsg());
            GetComponent<BoxCollider>().enabled = false;
        }

        if (gameObject.name == "CoinTutorial")
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    void WelcomMsg()
    {
        if (Manager.Instance.canStart)
        {
            if (welcomIndex <= 2)
            {
                instruction.text = welcomeMsgs[welcomIndex];
                clickImg.SetActive(true);
                freezeLikeImg.SetActive(true);
            }

            if (welcomIndex == 3)
            {
                player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;
                startPosition = player.GetComponent<Transform>().position;
                clickImg.SetActive(false);
                freezeLikeImg.SetActive(false);

                wasdCheck = true;

                instruction.text = welcomeMsgs[welcomIndex];
                welcomIndex++;
            }

            else if(wasdCheck)
            {
                WasdMoveChecker();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                welcomIndex++;
            }
        }
    }

    void WasdMoveChecker()
    {
        float distanceHorizontal = Mathf.Abs(player.GetComponent<Transform>().position.x - startPosition.x);
        float distanceVertical = Mathf.Abs(player.GetComponent<Transform>().position.z - startPosition.z);

        if (distanceHorizontal >= 5 || distanceVertical >= 5)
        {
            StartCoroutine(SuccessMsg());
            WelcomeMsgDone = true;
        }
    }

    void MouseMoveChecker()
    {
        instruction.text = "Look Around with your mouse for 3 second";

        float currentTime = Time.time;
        float slapsedTime = currentTime - startTime;
        player.gameObject.GetComponent<Transform>().position = currPosition;

        if (slapsedTime > 3.0)
        {
            // User has moved their mouse
            Debug.Log("User has moved their mouse");
            mouseCheckDone = true;
            freezeLikeImg.SetActive(false);
            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;
            StartCoroutine(SuccessMsg());
        }
    }

    void JumpChecker()
    {
        if (jumpIndex <= 1)
        {
            instruction.text = jumpMsgs[jumpIndex];
            clickImg.SetActive(true);
        }

        else if (jumpIndex == 2)
        {
            instruction.text = jumpMsgs[jumpIndex];
            clickImg.SetActive(true);
        }

        else if (jumpIndex == 3)
        {
            instruction.text = jumpMsgs[jumpIndex];
            jumpIndex++;

            freezeLikeImg.SetActive(false);
            clickImg.SetActive(false);
        }

        if (jumpIndex == 4)
        {
            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;    //allow player move;
            jumpCheckDone = true;
        }

        if (jumpIndex <= 5)
        {
            DontJumpOut();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            jumpIndex++;
        }
    }

    void SwingChecker()
    {
        if (swingIndex <= 4)
        {
            instruction.text = swingMsgs[swingIndex];
            clickImg.SetActive(true);
        }

        else if (swingIndex == 5)
        {
            instruction.text = swingMsgs[swingIndex];

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;    //allow player move;

            freezeLikeImg.SetActive(false);
            clickImg.SetActive(false);
            swingIndex++;
        }

        if (swingIndex <= 5)
        {
            DontJumpOut();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            swingIndex++;
        }
    }

    void CoinChecker()
    {
        ClearGameCheck();

        if (coinIndex <= 2)
        {
            instruction.text = coinMsgs[coinIndex];
            clickImg.SetActive(true);
        }

        else if (coinIndex == 3)
        {
            bossNumberText.SetActive(true);
            instruction.text = coinMsgs[coinIndex];
        }

        else if (coinIndex >= 4)
        {
            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;    //allow player move;
            freezeLikeImg.SetActive(false);
            clickImg.SetActive(false);

            if (coinIndex <= 5)
            {
                coinIndex++;
            }

        }

        if (coinIndex <= 4)
        {
            DontJumpOut();

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            coinIndex++;
        }

    }

    //Prevent a player jump out right after unfreezen
    void DontJumpOut()
    {
        player.gameObject.GetComponent<Transform>().position = currPosition;
        player.gameObject.GetComponent<PlayerMovement>().horizontalInput = 0;   //set input value 0 
        player.gameObject.GetComponent<PlayerMovement>().verticalInput = 0;
    }

    void DoMath(int num)
    {
        player.gameObject.GetComponent<Tutorial_math>().sum += num;
        player.gameObject.GetComponent<Tutorial_math>().mathCount--;
    }

    void ClearGameCheck()
    {
        int localSum = player.gameObject.GetComponent<Tutorial_math>().sum;
        int localCount = player.gameObject.GetComponent<Tutorial_math>().mathCount;
        worldSum = localSum;
        instruction.text = $"Current sum of your number: {worldSum}";

        if (localSum > bossNumber)
        {
            coinCheckDone = true;
            Manager.Instance.goalObject.SetActive(true);
        }

        if (localCount <= 0 && localSum <= bossNumber)
        {
            player.gameObject.GetComponent<Tutorial_math>().sum = 0;
            player.gameObject.GetComponent<Tutorial_math>().mathCount = 2;
        }
    }

    IEnumerator SuccessMsg()
    {
        instruction.text = "Great Job!";
        yield return new WaitForSeconds(3);
        instruction.text = "";
    }
}
