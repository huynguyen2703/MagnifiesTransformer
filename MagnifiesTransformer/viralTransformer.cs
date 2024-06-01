// Huy Quoc Nguyen
// CPSC 3200 P5 : Magnifier Transformer
/*
 * -----------------------------------------CLASS INVARIANT AND INTERFACE INVARIANT-----------------------------------------
 * ViralTransformer class represents a subtype of Transformer, it is a type of Transformer that also takes a target
 * integer and can perform different behaviors from its parent class. A ViralTransformer inherits all common
 * functionalities of its parent, Transformer. Aside from that, it provides its own implementation to the Transform()
 * functionality through dynamic binding (run-time polymorphism).
 * Over its lifetime a ViralTransformer can be active, inactive or shut down. ViralTransformer also allows the
 * client to reset it through Reset(), which inherited and overriden from the parent class. Client again need
 * to track on multiple states to ensure the dependencies and requests are valid to make the object works consistently.
 *
 * Aside from extended behaviors, all other functionalities of a ViralTransformer inherits from Transformer class
 * Details of the public functionalities and implementation are provided in the Transformer class.
 * Error handling for ViralTransformer is done through Exceptions.
 *
 * ViralTransformer is active if the modulo factor is valid, since invalid modulo factor could lead to
 * arithmetic errors and inconsistent values, passing invalid modulo factor will result in an exception being thrown.
 *
 * ViralTransformer(int targetValue, double moduloFactor) : Constructor
 *  - requires a modulo factor to be non-zero to ensure correct behaviors and protect object when performing
 *  arithmetic operations.
 *  - allows the client to create an active ViralTransformer object. This is where dependencies injection is expected.
 *  - the client needs to provide a target integer for the object to hold and a modulo factor, this will be used as
 *  a deciding factor for an operation when performing Transform(). This dependency is injected through constructor to
 *  ensure an internal pattern will be constructed.
 *
 * Transform(int guessValue)
 * - requires an object to be in active mode to perform actions.
 * - violation to the above condition will result in an exception being thrown.
 * - return three kind of values.
 * 0 if a guess matches the target value. Note that 0 could also comes from the fact that a target value is 0, client
 * must track on states to understand what is happening.
 * the product of the target value and a guess.
 * that same product but divide by a modulo factor provided in the constructor.
 *
 * Reset()
 * - wipe out all data used in AccelerateTransformer.
 * - used to stop the acceleration of the object and bring it back to initial state.
 *
 * 
 * All ViralTransformer properties to help the client keep track of new states and statistics are supported by the
 * parent class Transformer.
 * 
 * Other public methods and properties please see in Transformer class.
 */ 

namespace MagnifiesTransformer;

public class ViralTransformer : Transformer
{
    // data section
    private readonly int _targetValue;
    private readonly double _moduloFactor;
    
    
    // methods section
    /// PRECONDITIONS : modulo factor needs to be non-zero <para></para>
    /// POSTCONDITIONS : Object is correctly set in valid initial state, all private data are initialized with
    ///                  external dependencies
    public ViralTransformer(int targetValue, double moduloFactor) : base(targetValue)
    {
        CheckModuloFactor(moduloFactor);
        _targetValue = targetValue;
        _moduloFactor = moduloFactor;
        CurrentState = State.Active;
    }

    
    /// PRECONDITIONS : ViralTransformer object must be active <para></para>
    /// POSTCONDITIONS : a number will be sent back as a response <para></para>
    ///                  - 0 : correct guess <para></para>
    ///                  - the product of the target number and a guess <para></para>
    ///                  - the product modulo of the target value and a guess value
    public override double Transform(int guessValue)
    {
        if (TaskHelper(guessValue))
        {
            // only if a guess hits correctly
            return 0;
        }

        if (_targetValue <= _moduloFactor)
        {
            Operation = OperationType.Multiple;
            return _targetValue * guessValue;
        }
        // the only case _moduloFactor < _targetValue
        Operation = OperationType.Modulo;
        const int decimalPlaces = 2;
        return Math.Round((_targetValue * guessValue) % _moduloFactor, decimalPlaces);
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
        Operation = OperationType.Unknown;

    }
    
    // PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : None
    public override double GetModuloFactor()
    {
        const int decimalPlaces = 2;
        return Math.Round(_moduloFactor, decimalPlaces);
    }
    
    
    /// PRECONDITIONS : modulo factor needs to be positive <para></para>
    /// POSTCONDITIONS : No exception being thrown
    private void CheckModuloFactor(double moduloFactor)
    {
        if (moduloFactor == 0)
        {
            throw new Exception("modulo factor cannot be 0");
        }
    }

}

/*
 * -----------------------------------------IMPLEMENTATION INVARIANT----------------------------------------------------
 * A ViralTransformer object is dependent on the protected/private variables :
 * - _targetValue : an encapsulated integer injected via constructor
 * - _moduloFactor : a factor of type double that will help the object determine which operation to perform when
 *  calling Transform()
 *
 * Besides, there are public properties provided by the parent class to help the client interact and track the
 * object's multiple states. 
 * 
 * Details of the properties are provided in the Transformer parent class.
 *
 * ViralTransformer(int targetValue, double moduloFactor) : Constructor
 * - the constructor takes two argument, which is an encapsulated integer, and the other argument is a double number,
 * act as an modulo factor, this value is only valid when non-zero. Error processing is necessary here since, dividing
 * by 0 will result serious arithmetic error and destroy the object. An exception will be thrown if a modulo factor
 * is invalid.
 *
 * Transform(int targetValue)
 * - requirements are mostly alike to the Transform() version in the Transformer class. These include the object is in
 * active mode, verified by PreCheck() in the parent class. In addition, the small blocks of code are to
 * check if a guess matches the internal integer and to accumulate statistics, these are same and will be
 * carried by TaskHelper(int guessValue) in the parent class to reduce code complexity. The only difference lies in
 * the way the method responds the a guess. Instead of producing a difference or a sum between the target value and a
 * guess, it will return either a product of the target value and a guess or that product but divide by a modulo factor
 * and we take the remainder portion only. Private data _moduloFactor is crucial in deciding an operation to use, to
 * reduce confusion, a target value smaller and up to and including the modulo factor will result in a product is returned,
 * a product modulo will be returned otherwise.
 * This method will affect object's states because target integer can become known here.
 *
 * 
 * Reset()
 * - client can choose to reset an object, this method is extremely powerful, not only it will wipe out all accumulative
 * data, but it will bring object back to its initial state, which is active, so a current dead object can suddenly be
 * alive through this method.  Reset() is overriden from the parent class to contain more data to be reset.
 *
 * 
 * GetModuloFactor()
 * - this is an overridden version of GetModuloFactor() in the parent class Transformer. This method is used to
 * allow the children to access its methods through the parent interface inside a heterogeneous collection. The method
 * simply is a getter, it helps the client track an injected modulo factor after instantiating an object.
 *
 * 
 * CheckAccelerateFactor()
 * - this method is used for error processing, it validates a dependency injected through constructor, which is
 * a modulo factor. In order to pass this method, a modulo factor needs to be non-zero, otherwise an
 * exception will be thrown.
 */