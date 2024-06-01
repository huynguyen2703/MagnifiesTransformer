// Huy Quoc Nguyen
// CPSC 3200 P5 : Magnifier Transformer
/*
 *
 * -----------------------------------------CLASS INVARIANT AND INTERFACE INVARIANT-----------------------------------------
 * Transformer is a parent class of transformers family, this class represent a Transformer object
 * which can hold a target integer and performs various actions based on client's provided values and function calls.
 * Transformer class supports the guessing of an encapsulated value through method Transform(), the returned value is
 * determined based on external resources which will be provided by the client. Transformer is initially set in a valid
 * active state after instantiation, indicating that it is ready to perform action. During its lifetime, object can be
 * inactive or active, can be shut down if its encapsulated value is exposed, Transformer class allows that object to
 * be alive by using Reset(). The client must track on multiple states and dependencies in order for Transformer object
 * to work correctly. Transformer parent class provides multiple operations to use in assisting to respond a guess value.
 * These are Addition, Subtraction, Multiplication and Division. These operations are provided for both Transformer and
 * its descendants.

 * A Transformer object is active as long as it is instantiated or reset, since the limits relate to integers and decimal
 * numbers and operations only limited to addition and subtraction in the parent class, invalid resources would just be
 * any number outside of the the specified type. Object's states are manipulated via methods that could alter states,
 * such as Transform() and Reset, or Activate() and Deactivate(). Transformer provides some statistics to help the client
 * keep track of the results and actions made. These are the high and low data, supporting the guessing of the encapsulated
 * value after each call to Transform(). Besides, there are methods that require object to be in certain states, such as
 * a Transformer object needs to be in active mode to perform Transform(), or an object that is currently shut down
 * will not react to Activate() or Deactivate(). Any calls with invalid state will result in Exceptions being thrown as
 * warning.
 * Error handling for Transformer and its children is done through Exceptions.
 *
 * Transformer(int targetValue, double threshold = 0) : Constructor
 * - allows the client to create an active Transformer object. This is where dependencies
 * - injection is expected. The client will need to provide an integer to encapsulate and possibly and threshold, which
 * - will be used to determine actions of the object when performing Transform(). If no threshold is provided, default
 * - value 0 will be set to reduce client's responsibilities when instantiating child's objects.
 *
 * Transform(int guessValue)
 * - requires an object to be in active mode to perform actions.
 * - requires a guessValue is different from threshold since threshold is a boundary,
 * - a guess value below or above this boundary will result in different returned values.
 * - violations to any of the above conditions result in exceptions being thrown.
 * - return three different values based on external resources.
 * - 0 means a guess matches the encapsulated value.
 * - the sum of the target value and a guess.
 * - the difference of the target value and a guess.
 * - operation will be changed to Unknown and object will automatically shut down if target is guessed correctly.
 * - bringing object to life again requires explicit Reset()
 *
 * Reset()
 * - wipe out all data and put object into initial state.
 * - used to activate an object that is shut down.
 *
 * Activate()
 * - requires an object is not shut down.
 * - will throw exception if object is not in correct state.
 * - gives client the right to request state change.
 * - used to make an object active if not active.
 * - only accept request if state change is possible for object's current state.
 *
 * Deactivate()
 * - requires an object is not shut down.
 * - will throw exception if object is not in correct state.
 * - gives client the right to request state change.
 * - used to make an object inactive if not inactive.
 * - only accept request if state change is possible for object's current state.
 *
 *
 * Transformer also offers a variety of properties to help the client keep track of states and statistics.
 *  GetNumQueries : get the total number of queries to guess the target value.
 *  GetHighData : get the total number of high guesses.
 *  GetLowData : get the total number of low guesses.
 *  GetInitialState : check if object is in initial state.
 *  IsTargetKnown : check if the target value of object is exposed.
 *  GetState : get the current state of the object.
 *  GeOperationType : get the operation determined in the last call to Transform().
 *  IsActive : check if the object is active (more specific than GetState).
 *  IsInactive : check if the object is inactive (more specific than GetState).
 *  IsShutDown : check if the object is shut down (more specific than GetState).
 *  IsUnknown : check if an operation used in the last call to Transform() is not decided (more specific than GeOperationType).
 *  IsProduct : check if an operation used in the last call to Transform() is multiplication (more specific than GeOperationType).
 *  IsModulo : check if an operation used in the last call to Transform() is division (more specific than GeOperationType).
 *  IsSum : check if an operation used in the last call to Transform() is addition (more specific than GeOperationType).
 *  IsDifference : check if an operation used in the last call to Transform() is subtraction (more specific than GeOperationType).
 */

namespace MagnifiesTransformer;

public class Transformer
{
    // data section
    private readonly int _targetValue;
    private readonly double _threshold;
    protected uint NumQueries;
    protected uint HighData;
    protected uint LowData;
    protected bool InitialState;
    protected bool TargetKnown;
    protected State CurrentState;
    protected OperationType Operation;


    public enum State
    {
        // 0 : Inactive, 1 : Active, 2 : Shutdown
        Inactive,
        Active,
        ShutDown
    }

    public enum OperationType
    {
        // 0 : Unknown, 1 : Sum, 2 : Difference, 3 : Multiple, 4 : Modulo
        Unknown,
        Sum,
        Difference,
        Multiple,
        Modulo
    }

    // properties section
    public uint GetNumQueries => NumQueries;
    public uint GetHighData => HighData;
    public uint GetLowData => LowData;
    public bool GetInitialState => InitialState;
    public bool IsTargetKnown => TargetKnown;
    
    public State GetState => CurrentState;
    public OperationType GetOperationType => Operation;
    public bool IsActive => CurrentState == State.Active;
    public bool IsInactive => CurrentState == State.Inactive;
    public bool IsShutDown => CurrentState == State.ShutDown;
    public bool IsSum => Operation == OperationType.Sum;
    public bool IsDifference => Operation == OperationType.Difference;
    public bool IsUnknown => Operation == OperationType.Unknown;
    public bool IsProduct => Operation == OperationType.Multiple;
    public bool IsModulo => Operation == OperationType.Modulo;
    

    // methods section
    /// PRECONDITIONS: a threshold must not be the same as a target value <para></para>
    /// POSTCONDITIONS: object will be successfully created and set into a valid active state <para></para>
    ///                 all data fields initialized to reflect initial state
    ///
    public Transformer(int targetValue, double threshold = 0)
    {
        CheckThreshold(targetValue, threshold);
        // other data initialized to correct default values
        _targetValue = targetValue;
        _threshold = threshold;
        InitialState = true;
        TargetKnown = false;
        CurrentState = State.Active;
        Operation = OperationType.Unknown;
    }


    /// PRECONDITIONS : Transformer object must be active and a guess cannot be the same as the threshold <para></para>
    /// POSTCONDITIONS : a number will be sent back as a response <para></para>
    ///                  - 0 : correct guess <para></para>
    ///                  - the sum of the target value and a guess value <para></para>
    ///                  - the difference of the target value and a guess value
    public virtual double Transform(int guessValue)
    {
        if (TaskHelper(guessValue))
        {
            // only if a guess hits the target value correctly
            return 0;
        }

        if (guessValue > _threshold)
        {
            Operation = OperationType.Difference;
            return Math.Abs(_targetValue - guessValue);
        }

        if (guessValue < _threshold)
        {
            Operation = OperationType.Sum;
            return _targetValue + guessValue;
        }

        // if reached -> guessValue == threshold
        Operation = OperationType.Unknown;
        throw new Exception("Value collides with threshold");
    }


    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : All data will be wiped out and go back to their initial state. Object becomes active again.
    public virtual void Reset()
    {
        NumQueries = 0;
        HighData = 0;
        LowData = 0;
        CurrentState = State.Active;
        InitialState = true;
        TargetKnown = false;
        Operation = OperationType.Unknown;
    }


    /// PRECONDITIONS : The object which invokes the method needs to be an AccelerateTransformer <para></para>
    /// POSTCONDITIONS : None (exception thrown if not an AccelerateTransformer)
    public virtual double GetAcceleratingFactor()
    {
        if (GetType() != typeof(AccelerateTransformer))
        {
            throw new Exception("Object is not AccelerateTransformer");
        }

        return 0; // default value to complete implementation
    }


    /// PRECONDITIONS : The object which invokes the method needs to be an ModuloTransformer <para></para>
    /// POSTCONDITIONS : None (exception thrown if not a ModuloTransformer)
    public virtual double GetModuloFactor()
    {
        if (GetType() != typeof(ViralTransformer))
        {
            throw new Exception("Object is not ViralTransformer");
        }

        return 0; // default value to complete implementation
    }

    /// PRECONDITIONS : An object must not be in shut down mode <para></para>
    /// POSTCONDITIONS : An object that is not already active will become active again.
    public void Activate()
    {
        CheckDead();
        if (!IsInactive) return;
        InitialState = false;
        CurrentState = State.Active;
    }


    /// PRECONDITIONS : An object must not be in shut down mode <para></para>
    /// POSTCONDITIONS : An object that is not already inactive will become inactive again.
    public void Deactivate()
    {
        CheckDead();
        if (IsActive)
        {
            CurrentState = State.Inactive;
        }
    }

    
    /// PRECONDITIONS : an object must not be a TMagnifier <para></para>
    /// POSTCONDITIONS : object will be in shut down mode.
    public void ShutDown()
    {
        if (GetType() != typeof(TMagnifier))
        {
            CurrentState = State.ShutDown;
        }
    }
    
    /// PRECONDITIONS : An object must not be in shut down mode or inactive mode. <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void PreCheck()
    {
        if (IsInactive || IsShutDown)
        {
            throw new Exception("Invalid Request");
        }
    }


    /// PRECONDITIONS : An object must not be in shut down mode. <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void CheckDead()
    {
        if (IsShutDown)
        {
            throw new Exception("Object is currently shut down");
        }
    }


    /// PRECONDITIONS : A target value is different from a threshold. <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void CheckThreshold(int value, double threshold)
    {
        if ((value) == Convert.ToInt32(threshold)) // a target value is always integer -> ok to cast threshold
        {
            // to integer
            throw new Exception("Value collides with threshold");
        }
    }


    /// PRECONDITIONS : An object must be in active mode
    /// POSTCONDITIONS : a boolean value True or False will be returned indicating
    ///                  whether a guess hits the target correctly or not <para></para>
    ///                  an object may be shut down and data from a guess will be recorded and accumulated
    protected bool TaskHelper(int guessValue)
    {
        PreCheck();
        NumQueries += 1;
        InitialState = false;
        if (guessValue < _targetValue)
        {
            LowData += 1;
        }
        else if (guessValue > _targetValue)
        {
            HighData += 1;
        }

        if (guessValue == _targetValue)
        {
            TargetKnown = true;
            CurrentState = State.ShutDown;
            Operation = OperationType.Unknown;
            return true; // guess successful
        }

        return false;
    }
}

/*
 * -----------------------------------------IMPLEMENTATION INVARIANT----------------------------------------------------
 * A Transformer's states depends on the protected/private variables :
 * - _targetValue : an encapsulated integer injected via constructor.
 * - _threshold : a boundary used to determine operation used in each call to Transform().
 * - Operation : a variable which represents the operations used in Transformer class. Sum and Difference or Unknown,
 * - Product and Modulo used to support children.
 *
 * These values depend on each other and one may cause the other to change and hence, will change the entire state of
 * the object. Whether object is in active mode, inactive mode, or shut down will be based on these variables.
 *
 *
 * Besides, these variables below are responsible for recording object's states and data
 *  NumQueries : the total number of queries made to guess target value
 *  HighData : the total number of high guesses compared to target value
 *  LowData : the total number of low guesses compared to target value
 *  InitialState : a boolean to check if object is in initial state
 *  TargetKnown : a boolean to check if the target value is exposed
 *  CurrentState : a State variable is used to record state changes (values are from State enum)
 *  Operation : an OperationType variable is used to record operation changes (values are from OperationType enum)
 *
 * Transformer(int targetValue, double threshold = 0) : Constructor
 * - the constructor takes in one required argument, which is an encapsulated integer, and the other argument is a
 * threshold, which can be optional but expected to have to have a limit. These parameters are integers and double,
 * and their ranges are quite large, can be determined by the client. Error processing is done here to validate
 * dependencies, such as threshold and target value.
 *
 *
 * Transform(int guessValue)
 * - client can use this method to perform the guessing of the target integer, based on the threshold provided in the
 * constructor and guessValue, this method will determine which operations will be used to compute the returned value.
 * - either it will return the sum or the difference of the target integer and the guess value. The method requires an
 * object to be in active mode, hence it is checked at the beginning by PreCheck(). This method has small blocks of code
 * that will record some statistics, which are wrapped inside a helper TaskHelper(int guessValue). The correct guess
 * value will result in returned code 0. This method also requires that a guess value cannot be equal to a the specified
 * threshold. Because this threshold defines a limit for a guess, a guess that is higher or lower than that boundary
 * will help determine an operation used in a call to Transform(). An exception will be thrown if this condition is
 * violated. This method will affect object's states because target integer can become known here,
 * object can be shut down and operations alternatively replacing each other.
 * Transform() is set virtual for extension in child classes.
 *
 *
 * Reset()
 * - client can choose to reset an object, this method is extremely powerful, not only it will wipe out all accumulative
 * data, but it will bring object back to its initial state, which is active, so a current dead object can suddenly be
 * alive through this method.
 * Reset() is set virtual for extension in child classes.
 *
 *
 * GetAcceleratingFactor()
 * - this method acts as a support for its AccelerateTransformer class, this technique is to allow the use of
 * heterogeneous collection when a child class can use its overriden method through the parent in a heterogeneous
 * collection
 *
 * GetAcceleratingFactor()
 * - this method acts as a support for its ViralTransformer class, this technique is to allow the use of
 * heterogeneous collection when a child class can use its overriden method through the parent in a heterogeneous
 * collection
 *
 * Activate()
 * - client has the right to request state changes through this method. Activate() is used to explicitly turn an object
 * that is in inactive mode to become active again. Therefore, upon request, object's current state need to be checked
 * if it is in valid state to change. This is done with helper CheckDead(), which checks if object is currently shut
 * down.
 *
 *
 * Deactivate()
 * - client has the right to request state changes through this method. Deactivate() is used to explicitly turn an
 * object that is in active mode to become inactive again. Therefore, upon request, object's current state need to be
 * checked if it is in valid state to change. This is done with helper CheckDead(), which checks if object is currently
 * shut down.
 *
 *
 * PreCheck()
 * - this is a helper method used for error processing, this method targets the methods that require Transformer object
 * to be in active mode, therefore, it only checks if object is active, if it is not, it will throw an exception
 * indicating being in active mode must be met before performing certain actions or calling those methods that require
 * this condition.
 *
 *
 * CheckDead()
 * - this is a helper method used for error processing, this method targets Activate() and Deactivate(), since
 * Activate() and Deactivate() can only toggle the object's two states, it cannot do that if an object is being shut
 * down due to an its target value is exposed during some calls. Since data is recorded by TaskHelper(int guessValue),
 * the only way to bring object back to active mode is to reset everything through Reset() and wipe out all data.
 *
 *
 * CheckThreshold()
 * - this is a helper method used for error processing, this method verify dependencies injection, since a threshold
 * is a limit to determine operation used in each call to Transform(), it is not reasonable when a threshold is the
 * same as a guessValue as it will become difficult to determine an operation, this follows that if a target value is
 * same as a threshold, then a correct guess value can never be processed due to collision. Plus, it is reasonable that
 * a threshold acts as a factor to decide an operation used when a guess is not correct, therefore if a guess is correct
 * ,the guessing of the target value should stop and object is shut down. An assumption can be made is that we use this
 * method to check a guess value against a threshold or a target value against a threshold, and it is ok to cast a
 * threshold into an integer because the object always encapsulate an integer.
 *
 *
 * TaskHelper(int guessValue)
 * - this method is used to encapsulate some small blocks of code that can be reused within other Transform() methods
 * of child's classes. TaskHelper may change object's state as well because encapsulated part may change object's
 * to Shut Down and because TaskHelper is called within Transform(), it makes Transform() affect object's states more.
 *
 * ShutDown()
 * - this method is a helper method to support child object TMagnifier in the situation where Transformer needs to shut
 * itself down because that TMagnifier is shut down.
 */