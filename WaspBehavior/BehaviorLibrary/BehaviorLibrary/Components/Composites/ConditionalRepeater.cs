using System;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Actions;

namespace BehaviorLibrary
{
	public class ConditionalRepeater : BehaviorComponent
	{
		BehaviorAction _Updater;
		BehaviorAction _Action;
		
		private bool _ExecuteAction = true;
		
		/// <summary>
		/// Update and execute the action until updater tells to stop.
		/// 
		/// Updater must return: Running in order to execute action; Failure in order to stop (and nothing else);
		/// Action must return: Running in order to keep running; Success in order to executer updater; and Failure to stop.
		/// 
		/// </summary>
		/// <param name="updater"></param>
		/// <param name="action"></param>
		public ConditionalRepeater (BehaviorAction updater, BehaviorAction action) {
			this._Updater = updater;
			this._Action = action;
		}
		
		/// <summary>
		/// performs the given behavior
		/// </summary>
		/// <returns>the behaviors return code</returns>
		public override BehaviorReturnCode Behave(){

			if (_ExecuteAction) {
				ReturnCode = _Action.Behave();

			} else {
				ReturnCode = _Updater.Behave();
			}
			
			try{
				switch (ReturnCode) {
				// Stop behavioring.
				case BehaviorReturnCode.Failure:
					_ExecuteAction = false;
					ReturnCode = BehaviorReturnCode.Success;
					break;

				// Set to execute action in next interation.
				case BehaviorReturnCode.Running:
					_ExecuteAction = true;
					ReturnCode = BehaviorReturnCode.Running;
					break;

				// Set to execute updater in next interation.
				case BehaviorReturnCode.Success:
				default:
					_ExecuteAction = false;
					ReturnCode = BehaviorReturnCode.Running;
					break;
				}
				
			} catch (Exception e) {
				#if DEBUG
				Console.Error.WriteLine(e.ToString());
				#endif
				_ExecuteAction = false;
				ReturnCode = BehaviorReturnCode.Failure;
			}

			return ReturnCode;
		}
		
		
	}
}

