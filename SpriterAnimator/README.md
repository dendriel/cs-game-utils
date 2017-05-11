Spriter Animator

Files:
+ SpriterAnimator.cs
+ UnityAnimator.cs

This is a feature that mimics the Unity animator for Spriter animations.

The need for this feature arose when I was migrating unity animations with spritesheets to Spriter animations with bones. Spriter exports a whole prefab and provides some methods to control and trigger the animations. Its interface is different from Unity's and lack many features from Animator.

*In order to export Spriter animations to Unity you must use the SpriterDotNet implementation! SpriterAnimations depends on it.

So I built "SpriterAnimator" with 2 purposes in mind: replication most features from Animator and allowing to swap between Unity's animations and Spriter animations. Thus, SpriterAnimator constructors may receive a "Animator" from Unity or a "UnityAnimator" from Spriter.

The most complex feature in SpriterAnimator is the layers and what makes it hard to setup. But, when all of it is in place, it runs like a charm ;-]

I believe if you are looking for a feature like SpriterAnimator it's because you really want/need to use Spriter animations. So you will have to spend time learning how to configure it. I ensure you that it works! I have created this feature for Deceased Land (check the game at https://dendriel.github.io/#two).

*By the way, when using animations from Spriter in SpriterAnimator it's necessary to call its "Update" method every frame in order to handle automatic transitions from states.

While setting up the SpriterAnimator (with animations from Spriter), you "just" need to create the states and transitions. I will leave here the code used to setup the SpriterAnimator from the main character in Deceased Land.

Legend:

- STATE => STATE - Transitions by setting booleans!
- STATE -> STATE - Transitions by triggers!
- STATE (> STATE) - Automatic transitions! e.g.: start walking (> walking loop) 

You may want to draw the FSM to get a better view of it. =]

```C#
    // Animator definitions.
    // Bools and Triggers.
    protected const string walkBool = "Walk";
    protected const string runBool = "Run";
    protected const string attackTrigger = "Attack";
    protected const string jumpTrigger = "Jump";
    protected const string hurtTrigger = "Hurt";
    protected const string fallBool = "Fall";
    // States.
    protected const string idleStateTag = "Idle";
    protected const string walkStartStateTag = walkStateTag + "Start";
    protected const string walkStateTag = "Walk";
    protected const string walkEndStateTag = walkStateTag + "End";
    protected const string runStartStateTag = runStateTag + "Start";
    protected const string runStateTag = "Run";
    protected const string runEndStateTag = runStateTag + "End";
    protected const string attackStateTag = "Attack";
    protected const string jumpStateTag = "Jump";
    protected const string fallStateTag = "Fall";
    protected const string landStateTag = "Land";
    protected const string hurtStateTag = "Hurt";
    protected const string idleAttachStartStateTag = "IdleAttachStart";
    protected const string idleAttachStateTag = "IdleAttach";
    protected const string idleAttachEndStateTag = "IdleAttachEnd";
    protected const string walkAttachStartStateTag = "WalkAttachStart";    
    protected const string walkAttachStateTag = "WalkAttach";
    protected const string walkAttachEndStateTag = "WalkAttachEnd";
    // Layers.
    protected const int groundLayerIdx = 0;
    protected const int attachedLayerIdx = 1;
	

    protected override void SetupSpriterAnimator()
    {
        UnityAnimator spriterAnim = GetComponent<SpriterDotNetBehaviour>().Animator;
        Assert.IsNotNull<UnityAnimator>(spriterAnim, "SpriterDotNetBehaviour was not found!");

        // Set the spriter animator and the amount of layers.
        // Layer 0: base layer
        // Layer 1: attached layer.
        anim.SetupSpriterAnimator(spriterAnim, 2);

        SetupBaseLayer();
        SetupAttachedLayer();
    }

    void SetupBaseLayer()
    {
        int workingLayer = groundLayerIdx;

        anim.SpriterAnimatorSetupLayer(workingLayer,
            new string[2] { idleStateTag, idleStateTag },
            new string[2] { walkStartStateTag, walkStartStateTag },
            new string[2] { walkStateTag, walkStateTag },
            new string[2] { walkEndStateTag, walkEndStateTag },
            new string[2] { runStartStateTag, runStartStateTag },
            new string[2] { runStateTag, runStateTag },
            new string[2] { runEndStateTag, runEndStateTag },
            //new string[2] { attackStateTag, attackStateTag },
            new string[2] { jumpStateTag, jumpStateTag },
            new string[2] { fallStateTag, fallStateTag },
            new string[2] { landStateTag, landStateTag },
            // Attached States. Used to smooth transition between layers.
            new string[2] { walkAttachEndStateTag, walkAttachEndStateTag },
            new string[2] { idleAttachEndStateTag, idleAttachEndStateTag }
            );


        // WALKATTACHEND (> IDLEATTACHEND)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, walkAttachEndStateTag, idleAttachEndStateTag);
        // IDLEATTACHEND (> IDLE)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, idleAttachEndStateTag, idleStateTag);

        // Update animations speed.
        //anim.SpriterAnimatorSetupStateSpeed(groundLayerIdx, jumpStateTag, 0.5f);


        /* WALK STATE TRANSITIONS */
        // IDLE => WALKSTART
        anim.SpriterAnimatorSetupStateBool(workingLayer, idleStateTag, walkBool, true, walkStartStateTag);
        // WALKSTART (> WALK)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, walkStartStateTag, walkStateTag);
        // WALK => WALKEND
        anim.SpriterAnimatorSetupStateBool(workingLayer, walkStateTag, walkBool, false, walkEndStateTag);
        // WALKEND (> IDLE)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, walkEndStateTag, idleStateTag);
        // WALKEND => WALKSTART
        anim.SpriterAnimatorSetupStateBool(workingLayer, walkEndStateTag, walkBool, true, walkStartStateTag);
        
        /* RUN STATE TRANSITIONS. */
        // IDLE => RUNSTART
        anim.SpriterAnimatorSetupStateBool(workingLayer, idleStateTag, runBool, true, runStartStateTag);
        // RUNSTART (> RUN)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, runStartStateTag, runStateTag);
        // RUN => RUNEND
        anim.SpriterAnimatorSetupStateBool(workingLayer, runStateTag, runBool, false, runEndStateTag);
        // RUNEND (> IDLE)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, runEndStateTag, idleStateTag);
        // RUNDEND => RUNSTART
        anim.SpriterAnimatorSetupStateBool(workingLayer, runEndStateTag, runBool, true, runStartStateTag);

        // IDLE -> JUMP
        anim.SpriterAnimatorSetupStateTrigger(workingLayer, idleStateTag, jumpTrigger, jumpStateTag);
        // WALK -> JUMP
        anim.SpriterAnimatorSetupStateTrigger(workingLayer, walkStateTag, jumpTrigger, jumpStateTag);
        // RUN -> JUMP
        anim.SpriterAnimatorSetupStateTrigger(workingLayer, runStateTag, jumpTrigger, jumpStateTag);
        // RUNSTART -> JUMP
        anim.SpriterAnimatorSetupStateTrigger(workingLayer, runStartStateTag, jumpTrigger, jumpStateTag);
        // RUNEND -> JUMP
        anim.SpriterAnimatorSetupStateTrigger(workingLayer, runEndStateTag, jumpTrigger, jumpStateTag);

        // JUMP => LAND (this transition is needed in the cases which the player land before starting to fall)
        // Note that if the character is "landing on the air" is because the jump animation is finishing before the
        // character starts falling. This may be workarounded increasing the jump animation time (even if the last
        // frame stay a while longer).
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, jumpStateTag, landStateTag);        
        // ANYSTATE => FALL
        anim.SpriterAnimatorSetupStateBool(workingLayer, GenericAnimator.AnyStateLabel, fallBool, true, fallStateTag);
        // FALL -> LAND
        anim.SpriterAnimatorSetupStateBool(workingLayer, fallStateTag, fallBool, false, landStateTag);
        // LAND (> IDLE)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, landStateTag, idleStateTag);
    }

    void SetupAttachedLayer()
    {
        int workingLayer = attachedLayerIdx;

        anim.SpriterAnimatorSetupLayer(workingLayer,
            new string[2] { idleAttachStartStateTag, idleAttachStartStateTag },
            new string[2] { idleStateTag, idleAttachStateTag },
            new string[2] { walkStartStateTag, walkAttachStartStateTag },
            new string[2] { walkStateTag, walkAttachStateTag },
            new string[2] { walkEndStateTag, walkAttachEndStateTag }
            );

        /* WALK STATE TRANSITIONS */
        // IDLEATTACHSTART => IDLE
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, idleAttachStartStateTag, idleStateTag);

        /* WALK STATE TRANSITIONS */
        // IDLEATTACH => WALKATTACHSTART
        anim.SpriterAnimatorSetupStateBool(workingLayer, idleStateTag, walkBool, true, walkStartStateTag);
        // WALKATTACHSTART (> WALK)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, walkStartStateTag, walkStateTag);
        // WALK => WALKEND
        anim.SpriterAnimatorSetupStateBool(workingLayer, walkStateTag, walkBool, false, walkEndStateTag);
        // WALKEND (> IDLE)
        anim.SpriterAnimatorSetupAutomaticTransition(workingLayer, walkEndStateTag, idleStateTag);
        // WALKEND => WALKSTART
        anim.SpriterAnimatorSetupStateBool(workingLayer, walkEndStateTag, walkBool, true, walkStartStateTag);
    }
```