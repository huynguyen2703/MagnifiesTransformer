// Huy Quoc Nguyen
// CPSC 3200 P5 : Magnifier Transformer
/*
 * -----------------------------------------CLASS INVARIANT AND INTERFACE INVARIANT-----------------------------------------
 * AccelerateTransformer class represents a subtype of Transformer, it is a type of Transformer that also takes a target
 * integer and can return accelerated values from the target value it encapsulates until it is reset. An
 * AccelerateTransformer inherits all common functionalities of its parent, Transformer. Aside from that,
 * it provides its own implementation to the Transform() functionality through dynamic binding (run-time polymorphism).
 * Over its lifetime an AccelerateTransformer can be active, inactive or shut down. AccelerateTransformer also allows the
 * client to reset it through Reset() inherited and overriden from the parent class. Client again need to track on multiple
 * states to ensure the dependencies and requests are valid to make the object works consistently. Note that
 * accelerateTransformer accelerates demonstrates operation Sum.
 *
 * Aside from extended behaviors, all other functionalities of an AccelerateTransformer inherits from Transformer class
 * Details of the public functionalities and implementation are provided in the Transformer class.
 * Error handling for AccelerateTransformer is done through Exceptions.
 *
 * AccelerateTransformer is active if the accelerator factor is valid, since invalid accelerateFactor could lead to
 * arithmetic errors and break the system, passing invalid accelerateFactor will result in an exception being thrown.
 * 
 * AccelerateTransformer(int targetValue, double accelerateFactor) : Constructor
 * - requires an accelerateFactor to be positive to ensure correct behaviors when performing arithmetic operations.
 * - allows the client to create an active AccelerateTransformer object. This is where dependencies injection is expected.
 * - the client needs to provide a target integer for the object to hold and an accelerateFactor to support acceleration
 * - of the object when calling Transform(). This dependency is injected through constructor to
 * ensure a consistent internal pattern.
 *
 * Transform(int guessValue)
 * - requires an object to be in active mode to perform actions.
 * - violation to the above condition will result in an exception being thrown.
 * - return two kind of values, 0 if a guess matches the target value, else it returns the target value.
 * - but in accelerating condition (which means the exact target value will not be returned but its accelerating version).
 *
 * Reset()
 * - wipe out all data used in AccelerateTransformer.
 * - used to stop the acceleration of the object and bring it back to initial state.
 * 
 * All AccelerateTransformer properties to help the client keep track of new states and statistics are supported by the
 * parent class Transformer.
 * 
 * Other public methods please see in Transformer class.
 * 
*/

namespace MagnifiesTransformer;

public class AccelerateTransformer : Transformer
{
    // data section
    private readonly int _targetValue;
    private readonly double _accelerateFactor;
    private double _accelerateValue; // shadow of _targetValue
    
    
    // methods section
    /// PRECONDITIONS : accelerateFactor must be positive <para></para>
    /// POSTCONDITIONS : Object is correctly set in valid initial state, all private data are initialized with
    ///                  external dependencies
    public AccelerateTransformer(int targetValue, double accelerateFactor) : base(targetValue)
    {
        CheckAccelerateFactor(accelerateFactor);
        
        _targetValue = targetValue;
        _accelerateValue = targetValue;
        _accelerateFactor = accelerateFactor;
        CurrentState = State.Active;
        Operation = OperationType.Unknown;
    }

    
    /// PRECONDITIONS : AccelerateTransformer must be in active mode <para></para>
    /// POSTCONDITIONS : A number will be sent back as a response <para></para>
    ///                  - 0 : correct guess <para></para>
    ///                  - an accelerated version of the target value
    public override double Transform(int guessValue)
    {
        if (TaskHelper(guessValue))
        {
            // only if a guess hits correctly
            return 0;
        }

        Operation = OperationType.Sum;
        return _accelerateValue += _accelerateFactor;
    }

    
    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : All data will be wiped out and go back to their initial state. Object becomes active again.
    public override void Reset()
    {
        NumQueries = 0;
        HighData = 0;
        LowData = 0;
        CurrentState = State.Active;
        InitialState = true;
        TargetKnown = false;
        _accelerateValue = _targetValue;
        Operation = OperationType.Unknown;
    }

    
    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : None
    public override double GetAcceleratingFactor()
    {
        return _accelerateFactor;
    }

    
    /// PRECONDITIONS : accelerateFactor needs to be positive <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void CheckAccelerateFactor(double accelerateFactor)
    {
        if (accelerateFactor <= 0)
        {
            throw new Exception("accelerator factor must be positive");
        }
    }
}

/*
 * -----------------------------------------IMPLEMENTATION INVARIANT----------------------------------------------------
 * An AccelerateTransformer object is dependent on the protected/private variables :
 * - _targetValue : an encapsulated integer injected via constructor
 * - _accelerateFactor : a factor of type double that will help the object accelerate when performing Transform()
 * _accelerateValue : represent targetValue in accelerating condition, this variable helps keeping targetValue safe,
 * also helpful when resetting the object.
 *
 * 
 * Besides, there are public properties provided by the parent class to help the client interact and track the
 * object's multiple states. 
 * Details of the properties are provided in the Transformer parent class.
 *
 * AccelerateTransformer(int targetValue, double accelerateFactor) : Constructor
 * - the constructor takes two argument, which is an encapsulated integer, and the other argument is a double number,
 * act as an accelerating factor, this value is only valid when positive. Error processing is necessary here since, 0
 * or negative accelerating factor will result in the object accelerating backward or not accelerating at all. An
 * exception will be thrown if an accelerating factor is invalid.
 *
 * Transform(int targetValue)
 * - requirements are mostly alike to the Transform() version in the Transformer class. These include the object is in
 * active mode, verified by PreCheck() in the parent class. In addition, the small blocks of code are to
 * check if a guess matches the internal integer and to accumulate statistics, these are same and will be
 * carried by TaskHelper(int guessValue) in the parent class to reduce code complexity. The only difference lies in
 * the way the method responds the a guess. Instead of producing a difference or a sum between the target value and a
 * guess, it will return a version of the target value but in accelerating condition, each call to Transform() that
 * provides a guess value that does not match with the target value will result in the object accelerates the target
 * value by an accelerating factor provided in the constructor, this internal pattern is consistent throughout the
 * lifetime of the object, and the object only stops accelerating once it is reset or its target value is exposed.
 * This method will affect object's states because target integer can become known here. 
 *
 * Reset()
 * - client can choose to reset an object, this method is extremely powerful, not only it will wipe out all accumulative
 * data, but it will bring object back to its initial state, which is active, so a current dead object can suddenly be
 * alive through this method. This method is used to stop an AccelerateTransformer object from accelerating. Reset()
 * overriden from the parent class to contain more data to be reset.
 *
 * GetAcceleratingFactor()
 * - this is an overridden version of GetAcceleratingFactor() in the parent class Transformer. This method is used to
 * allow the children to access its methods through the parent interface inside a heterogeneous collection. The method
 * simply is a getter, it helps the client track an injected accelerating factor after instantiating an object.
 * 
 * CheckAccelerateFactor()
 * - this method is used for error processing, it validates a dependency injected through constructor, which is
 * an accelerating factor. In order to pass this method, an accelerating factor needs to be positive, otherwise an
 * exception will be thrown.
*/