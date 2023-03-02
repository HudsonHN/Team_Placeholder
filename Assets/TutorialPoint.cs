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
    };

    //Mouse move checker
    private bool mouseCheckDone = true;
    private Vector3 currPosition;   //to make player cant move on mouse section;


    //jump checker
    private bool jumpCheckDone = true;
    private GameObject luanchBar;
    Slider slider;
    private int jumpIndex = 0;
    string[] jumpMsgs =
    {
        "Let's learn how to LAUNCH",
        "Look where you want to jump",
        "Press and hold SPACE BAR",
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
        "Release after you made enough momentum ",
        "Jump further with W Key",
        "Try yourself",
    };

    //coin get check
    private bool coinCheckDone = true;
    private int coinIndex = 0;
    string[] coinMsgs =
    {
        "Final mission of Tutorial 1",
        "Collect all COINs to clear",
        "Be careful the LASER WALL!",
        "Go and get all COINs!",
    };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCapsule");
        freezeLikeImg = clickImg = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("ScreenFreeze").gameObject;
        clickImg = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("Left_Click").gameObject;
        luanchBar = GameObject.Find("UI").transform.Find("DetailedTutorial").transform.Find("LaunchBar").gameObject;
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
            slider = luanchBar.GetComponent<Slider>();

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;

            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            player.gameObject.GetComponent<Transform>().position = currPosition;
            DontJumpOut();
            
        }

        if (gameObject.name == "SwingTutorial")
        {
            swingCheckerDone = false;
            freezeLikeImg.SetActive(true);

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;
            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            player.gameObject.GetComponent<Transform>().position = currPosition;

            DontJumpOut();
        }

        if (gameObject.name == "CoinTutorial")
        {
            coinCheckDone = false;
            freezeLikeImg.SetActive(true);

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = false;    //stop player move;
            currPosition = player.gameObject.GetComponent<Transform>().position;    //stop player at the current position
            player.gameObject.GetComponent<Transform>().position = currPosition;

            DontJumpOut();
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
            if (welcomIndex <= 1)
            {
                instruction.text = welcomeMsgs[welcomIndex];
                clickImg.SetActive(true);
                freezeLikeImg.SetActive(true);
            }

            if (welcomIndex == 2)
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
        instruction.text = "Look Around with your mouse for 5 second";

        float currentTime = Time.time;
        float slapsedTime = currentTime - startTime;
        player.gameObject.GetComponent<Transform>().position = currPosition;

        if (slapsedTime > 5.0)
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
            player.gameObject.GetComponent<Transform>().position = currPosition;

            instruction.text = jumpMsgs[jumpIndex];
            clickImg.SetActive(true);
        }

        else if (jumpIndex == 2)
        {
            player.gameObject.GetComponent<Transform>().position = currPosition;
            DontJumpOut();

            instruction.text = jumpMsgs[jumpIndex];
            clickImg.SetActive(true);
            slider.value = player.gameObject.GetComponent<PlayerMovement>().launchHoldTimer;
        }

        else if (jumpIndex == 3)
        {
            if (slider.value < 1.0)
            {
                instruction.text = jumpMsgs[jumpIndex];
            }
            else
            {
                instruction.text = "Release SPACE BAR";
                jumpIndex++;
            }

            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;    //allow player move;

            freezeLikeImg.SetActive(false);
            clickImg.SetActive(false);
            luanchBar.SetActive(true);
            slider.value = player.gameObject.GetComponent<PlayerMovement>().launchHoldTimer;
        }

        if (jumpIndex == 4)
        {
            luanchBar.SetActive(false);
            jumpCheckDone = true;
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
            player.gameObject.GetComponent<Transform>().position = currPosition;

            instruction.text = swingMsgs[swingIndex];
            clickImg.SetActive(true);
        }

        else if (swingIndex == 5)
        {
            instruction.text = swingMsgs[swingIndex];

            DontJumpOut();
            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;    //allow player move;

            freezeLikeImg.SetActive(false);
            clickImg.SetActive(false);
            swingIndex++;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            swingIndex++;
        }
    }

    void CoinChecker()
    {
        if (coinIndex <= 2)
        {
            player.gameObject.GetComponent<Transform>().position = currPosition;

            instruction.text = coinMsgs[coinIndex];
            clickImg.SetActive(true);
        }

        else if (coinIndex == 3)
        {
            DontJumpOut();
            instruction.text = coinMsgs[coinIndex];

            swingIndex++;
        }

        else if (coinIndex >= 4)
        {
            player.gameObject.GetComponent<PlayerMovement>().movePlayer = true;    //allow player move;

            freezeLikeImg.SetActive(false);
            clickImg.SetActive(false);

            instruction.text = $"Left coins: {Manager.Instance.coinsInLevel}";
        }

        if (Manager.Instance.coinsInLevel <= 0)
        {
            coinCheckDone = true;
            StartCoroutine(SuccessMsg());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            coinIndex++;
        }
    }

    //Prevent a player jump out right after unfreezen
    void DontJumpOut()
    {
        player.gameObject.GetComponent<PlayerMovement>().horizontalInput = 0;   //set input value 0 
        player.gameObject.GetComponent<PlayerMovement>().verticalInput = 0;
    }
    IEnumerator SuccessMsg()
    {
        instruction.text = "Great Job!";
        yield return new WaitForSeconds(3);
        instruction.text = "";
    }
}
