using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{

    public static UserInput instance;

    public Vector2 MoveInput {  get; private set; }
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }

    public bool PauseInput { get; private set; }
    public bool NextCaseInput { get; private set; }
    public bool PreviousCaseInput { get; private set; }
    public bool ConsumeItemInput { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction pauseAction;
    private InputAction previousCaseAction;
    private InputAction nextCaseAction;
    private InputAction consumeItemAction;
    void Awake()
    {
        if(instance == null) 
            instance = this;

        playerInput = GetComponent<PlayerInput>();

        SetupInputActions();

    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        pauseAction = playerInput.actions["Pause"];
        previousCaseAction = playerInput.actions["PreviousCase"];
        nextCaseAction = playerInput.actions["NextCase"];
        consumeItemAction = playerInput.actions["ConsumeItem"];
    }

    private void UpdateInputs()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        JumpInput = jumpAction.IsPressed();
        DashInput = dashAction.IsPressed();
        PauseInput = pauseAction.WasPressedThisFrame();
        PreviousCaseInput = previousCaseAction.WasPressedThisFrame();
        NextCaseInput = nextCaseAction.WasPressedThisFrame();
        ConsumeItemInput = consumeItemAction.WasPressedThisFrame();
    }
}
