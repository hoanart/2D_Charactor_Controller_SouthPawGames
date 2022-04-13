using UnityEngine.InputSystem;

interface IActable {
   void OnAct(InputAction.CallbackContext context);

   void OnStop(InputAction.CallbackContext context);

}
