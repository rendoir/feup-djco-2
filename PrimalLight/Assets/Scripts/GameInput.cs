using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameInput : MonoBehaviour
{
    static GameInput current;

	static public float horizontal;
    static public float vertical;
    static public float rotation;
    static public float sprint;
	static public bool jumpPressed;
    static public bool attackHeld;
    static public bool interactPressed;
	static public ColorInput colorInput;
	
	public struct ColorInput {
		public bool R;
		public bool G;
		public bool B;
		public bool A;
	}
	
	bool readyToClear;								//Bool used to keep input in sync

    void Awake()
	{
		//If a Game Input exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game Input
			Destroy(gameObject);
			return;
		}

		//Set this as the current game input
		current = this;

		//Persist this object between scene reloads
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		//Clear out existing input values
		ClearInput();

		//Process keyboard and mouse inputs buttons
		ProcessButtons();
	}

	void FixedUpdate()
	{
		//Process keyboard and mouse inputs axis
        ProcessAxis();

		readyToClear = true;
	}

	void ClearInput()
	{
		//If we're not ready to clear input, exit
		if (!readyToClear)
			return;

		//Reset all inputs
		jumpPressed		= false;
        attackHeld      = false;
        interactPressed = false;
		colorInput.R 	= false;
		colorInput.G 	= false;
		colorInput.B 	= false;
		colorInput.A 	= false;
		
		readyToClear	= false;
	}

	void ProcessButtons()
	{
		//Accumulate button inputs
		jumpPressed     = jumpPressed     || Input.GetButtonDown("Jump");
        interactPressed = interactPressed || Input.GetButtonDown("Interact");
        attackHeld	    = attackHeld      || Input.GetButton("Attack");

		//Accumulate color inputs
		colorInput.R = colorInput.R || Input.GetKey(KeyCode.R);
		colorInput.G = colorInput.G || Input.GetKey(KeyCode.G);
		colorInput.B = colorInput.B || Input.GetKey(KeyCode.B);
		colorInput.A = colorInput.A || Input.GetKey(KeyCode.A);
	}

    void ProcessAxis()
    {
        //Get axis input
		horizontal = Input.GetAxis("Horizontal");
        vertical   = Input.GetAxis("Vertical");
        rotation   = Input.GetAxis("Rotation");
        sprint     = Input.GetAxis("Sprint");
    }
}
