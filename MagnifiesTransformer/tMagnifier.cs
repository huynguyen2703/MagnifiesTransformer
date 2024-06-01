// Huy Quoc Nguyen
// CPSC 3200 P5 : Magnifier Transformer

/*
 * -----------------------------------------CLASS INVARIANT AND INTERFACE INVARIANT-------------------------------------
 * TMagnifier is a child class of both a Transformer and a Magnifier, this class is treated as an intersection of
 * Transformer and Magnifier. A TMagnifier object is capable of doing everything a Transformer and a Magnifier can do,
 * demonstrating multiple inheritance. Since TMagnifier is a child of Magnifier and Transformer, its states are
 * represented by both parents, it can holds a private integer and perform transformations with that value and it can
 * also scales an encapsulated value. TMagnifier is initially set in a valid active state after instantiation, indicating
 * that is is ready to perform action. During its lifetime, object can be active or inactive, it can be shut down due to
 * two possibilities. The first possibility is its private value is exposed, which is Transformer characteristic, or the
 * number of chances to yield a scaled size is exhausted, which is characteristic of Magnifier. Again, the client must
 * track on multiple states, now not only with Magnifier states but also Transformer states because all of them represent
 * TMagnifier states. Luckily, a Transformer and a Magnifier are very similar, so there are many overlapping states,
 * therefore, there is one state less for the client to worry about. One special thing about a random TMagnifier is that
 * it can hold the power of any Transformer, since there are three Transformer types in the Transformer family, and all
 * of them are considered Transformer, there is a possibility that one TMagnifier could be an AccelerateTransformer or
 * it could be ViralTransformer or a normal Transformer, and of course with all Magnifier powers. Therefore, in order to
 * differentiate this special characteristic, TMagnifier class provides a method GetTransformerType(), which could help
 * the user determines what kind of transformer of the object who triggers the method, it may be obvious from the inside,
 * but if the client looks at all TMagnifier at once, each with a different Transformer type, this method can be very
 * handy.
 *
 * A TMagnifier is active as long as it is instantiated or reset, since the Magnifier object is what a TMagnifier is more
 * alike, the client needs to provide all argument required to create a Magnifier in order for TMagnifier to directly
 * inherit from Magnifier. About how to get Transformer abilities, TMagnifier will adopt Transformer as a sub-object
 * and exploit all of its power by borrowing them and use for TMagnifier special methods. Therefore, TMagnifier can
 * perform transformation through TMagnifierTransform(int guessValue), activate and deactivate the object through
 * TMagnifierActivate() and TMagnifierDeactivate(), as well as resetting the object with TMagnifierReset(). Additionally,
 * If a TMagnifier is an AcceleratingTransformer or a ViralTransformer, it will be able to use either method
 * GetAcceleratingFactor() or GetModuloFactor(). TMagnifier also has TMagnifierYieldSize(), the reason a TMagnifier needs
 * to use this method to activate YieldSize() is because it needs to resolve overlapping states with Transformer sub-object.
 *
 * 
 * TMagnifier(uint size, double scaleFactor, uint limit, uint maxYield, Transformer transformer) : Constructor
 * - it will help initialize the data for Magnifier parent so that TMagnifier can use the functionalities.
 * - Also, it requires one additional argument which is a Transformer object, this is how we will be able to use
 * - Transformer power with TMagnifier.
 *
 * 
 * TMagnifierYieldSize()
 * - this method will activate Magnifier's YieldSize(), but at the same time it will check for any state changes and
 * resolve overlapping states with Transform() from Transformer.
 * - (ex : TMagnifier is shut down from calling this method, this means Transformer sub-object needs to shut down too to
 * - prevent TMagnifier from performing Transform() when it is being shut down.
 *
 *
 * TMagnifierTransform()
 * - this method will activate Transformer's Transform(), but at the same time it will check for any state changes and
 * resolve overlapping states with YieldSize() from Magnifier.
 * - (ex : TMagnifier is shut down from calling this method, this means TMagnifier needs to shut down too to
 * - prevent itself from performing YieldSize() when it is being shut down.
 *
 * 
 * TMagnifierReset()
 * - this method will reset everything TMagnifier has to initial state.
 * - this means Transformer sub-object will be reset too since its states also represent TMagnifier's states.
 *
 * 
 * TMagnifierActivate()
 * - this method will activate TMagnifier if it is not already in active mode.
 * - this means Transformer sub-object will be activated too since its states also represent TMagnifier's states.
 *
 * 
 * TMagnifierDeactivate()
 * - this method will deactivate TMagnifier if it is not already in inactive mode.
 * - this means Transformer sub-object will be deactivated too since its states also represent TMagnifier's states.
 *
 * 
 * GetAcceleratingFactor()
 * - this method will get the accelerating factor if a TMagnifier is an AcceleratingTransformer.
 *
 * 
 * GetModuloFactor()
 * - this method will get the modulo factor if a TMagnifier is an ViralTransformer.
 *
 * 
 * GetTransformerType()
 * - this method will help the client determine the Transformer type of the specific TMagnifier they are working with.
 *
 * 
 * TMagnifier also offers a variety of properties to help the client keep track of states and statistics.
 *
 * TMagnifierGetHighData : get the total number of high guesses  
 * TMagnifierGetLowData : get the total number of low guesses
 * TMagnifierIsTargetKnown : check if the target integer is known
 * TMagnifierSum : check if the last operation is addition
 * TMagnifierDifference : check if the last operation is subtraction
 * TMagnifierProduct : check if the last operation is multiplication
 * TMagnifierModulo : check if the last operation is modularization
 * TMagnifierUnknownOperation : check if the last operation is unknown (no calls to Transform yet)
 * TMagnifierOperationType : get the current operation 
 */

namespace MagnifiesTransformer;

public class TMagnifier : Magnifier
{
    // data section
    private readonly Transformer _transformer;
    
    // properties section

    public uint TMagnifierGetHighData => _transformer.GetHighData;
    public uint TMagnifierGetLowData => _transformer.GetLowData;
    public bool TMagnifierIsTargetKnown => _transformer.IsTargetKnown;
    public bool TMagnifierSum => _transformer.IsSum;
    public bool TMagnifierDifference => _transformer.IsDifference;
    public bool TMagnifierProduct => _transformer.IsProduct;
    public bool TMagnifierModulo => _transformer.IsModulo;
    public bool TMagnifierUnknownOperation => _transformer.IsUnknown;
    public Transformer.OperationType TMagnifierOperationType => _transformer.GetOperationType;
    
    
    // methods section
    /// PRECONDITIONS: a scale factor must be nonnegative <para></para>
    ///                a Transformer object passed in must be a valid Transformer object
    /// POSTCONDITIONS: object will be successfully created and set into a valid active state <para></para>
    ///                 all data fields initialized to reflect initial state
    ///
    public TMagnifier(uint size, double scaleFactor, uint limit, uint maxYield, Transformer transformer) :
        base(size, scaleFactor, limit, maxYield)
    { 
        _transformer = transformer;
    }

    /// PRECONDITIONS: TMagnifier must be active <para></para>
    /// POSTCONDITIONS: object will be successfully created and set into a valid active state <para></para>
    ///                 all data fields initialized to reflect initial state
    public double TMagnifierYieldSize()
    {
        double returnVal = YieldSize();
        if (GetState == State.ShutDown)
        {
            _transformer.ShutDown();
        }

        return returnVal;
    }
    
    /// PRECONDITIONS : TMagnifier and Transformer objects must be active and a guess cannot be the same as the threshold <para></para>
    /// POSTCONDITIONS : a number will be sent back as a response <para></para>
    ///                  - 0 : correct guess <para></para>
    ///                  - the sum of the target value and a guess value <para></para>
    ///                  - the difference of the target value and a guess value
    public double TMagnifierTransform(int guessValue)
    {
        _numQueries += 1;
        _initialState = false;
        double returnVal = _transformer.Transform(guessValue);
        if (_transformer.IsShutDown)
        {
            ShutDown();
        }

        return returnVal;
    }
    
    
    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : All data will be wiped out and go back to their initial state. Object becomes active again.
    public void TMagnifierReset()
    {
        _transformer.Reset();
        Reset(); // magnifier version
    }
    
    
    /// PRECONDITIONS : An object must not be in shut down mode <para></para>
    /// POSTCONDITIONS : An object that is not already active will become active again.
    public void TMagnifierActivate()
    {
        _transformer.Activate();
        Activate(); // magnifier version
    }
    
    
    /// PRECONDITIONS : An object must not be in shut down mode <para></para>
    /// POSTCONDITIONS : An object that is not already inactive will become inactive again.
    public void TMagnifierDeactivate()
    {
        _transformer.Deactivate();
        Deactivate(); // magnifier version
    }
    
    
    /// PRECONDITIONS : The object which invokes the method needs to be an AccelerateTransformer <para></para>
    /// POSTCONDITIONS : None (exception thrown if not an AccelerateTransformer)
    public double GetAcceleratingFactor()
    { 
        return _transformer.GetAcceleratingFactor();
    }
    
    
    /// PRECONDITIONS : The object which invokes the method needs to be an ModuloTransformer <para></para>
    /// POSTCONDITIONS : None (exception thrown if not a ModuloTransformer)
    public double GetModuloFactor()
    { 
        return _transformer.GetModuloFactor();
    }
    
    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : Type of the Transformer sub-object will be returned.
    public Transformer GetTransformerType()
    {
        return _transformer;
    }
}

/*
 * -----------------------------------------IMPLEMENTATION INVARIANT----------------------------------------------------
 * A TMagnifier does not have real data since it inherits from Magnifier and it also uses a sub-object Transformer, hence
 * the only thing it needs to depend on is
 * _transformer : a transformer object that will help TMagnifier performs Transformer abilities.
 *
 * TMagnifier(uint size, double scaleFactor, uint limit, uint maxYield, Transformer transformer) : Constructor
 * - it will help initialize the data for Magnifier parent so that TMagnifier can use the functionalities.
 * - Also, it requires one additional argument which is a Transformer object, this is how we will be able to use
 * - Transformer power with TMagnifier.
 *
 * 
 * TMagnifierYieldSize()
 * - this method will activate Magnifier's YieldSize(), but at the same time it will check for any state changes and
 * resolve overlapping states with Transform() from Transformer()
 * - (ex : TMagnifier is shut down from calling this method, this means Transformer sub-object needs to shut down too to
 * - prevent TMagnifier from performing Transform() when it is being shut down.
 *
 * TMagnifierTransform()
 * - this method will activate Transformer's Transform(), but at the same time it will check for any state changes and
 * resolve overlapping states with YieldSize() from Magnifier.
 * - (ex : TMagnifier is shut down from calling this method, this means TMagnifier needs to shut down too to
 * - prevent itself from performing YieldSize() when it is being shut down.
 *
 * 
 * TMagnifierReset()
 * - this method will reset everything TMagnifier has to initial state.
 * - this means Transformer sub-object will be reset too since its states also represent TMagnifier's states.
 *
 * 
 * TMagnifierActivate()
 * - this method will activate TMagnifier if it is not already in active mode.
 * - this means Transformer sub-object will be activated too since its states also represent TMagnifier's states.
 *
 * 
 * TMagnifierDeactivate()
 * - this method will deactivate TMagnifier if it is not already in inactive mode.
 * - this means Transformer sub-object will be deactivated too since its states also represent TMagnifier's states.
 *
 * 
 * GetAcceleratingFactor()
 * - this method will get the accelerating factor if a TMagnifier is an AcceleratingTransformer.
 * - an exception will be thrown if it is not the correct type
 *
 * 
 * GetModuloFactor()
 * - this method will get the modulo factor if a TMagnifier is an ViralTransformer.
 * - an exception will be thrown if it is not the correct type
 * 
 * 
 * GetTransformerType()
 * - this method will help the client determine the Transformer type of the specific TMagnifier they are working with.
 * - it will compare the type of the the sub-object the TMagnifier is holding with the correct type to get the value
 * - back.
 * (Notes : more information regarding inner methods please see three Transformer classes and Magnifier class)
 */