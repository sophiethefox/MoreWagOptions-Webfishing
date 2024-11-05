using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace MoreWag;

public class TailRootPatch(Config config) : IScriptMod
{
    private readonly Config _config = config;

    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/tail_root.gdc";
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // rot.rotation_degrees = des_rot
        var rotWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "rot" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "rotation_degrees" },
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken { Name: "des_rot" },
        ]);

        // var wag = 0.0
        var wagWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken { Name: "wag" },
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken { Value: RealVariant { Value: 0.0 } },
        ]);

        // 	else : wag = sin(OS.get_ticks_msec() * 0.014) * 1.0

        var stepAmount = _config.SpeedStep;

        foreach (var token in tokens)
        {
            yield return token;

            if(wagWaiter.Check(token))
            {
                // var lastStepTime = 0
                yield return new Token(TokenType.Newline);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("lastStepTime");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));
                // var multiplier = 0.014
                yield return new Token(TokenType.Newline);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("multiplier");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new RealVariant(0.014));
            }

            if (rotWaiter.Check(token))
            {
                /*
                 * 	# more than 100ms progressed
	                if OS.get_ticks_msec() - lastStepTime > 100:
		                lastStepTime = OS.get_ticks_msec()
		                # increment keybind pressed
		                if Input.is_action_pressed("wag_faster"):
			                multiplier = clamp(multiplier + {stepAmount}, 0, 1)
		                if Input.is_action_pressed("wag_slower"):
			                multiplier = clamp(multiplier - {stepAmount}, 0, 1)
	                # adjust the wag speed
	                if wagging:
		                wag = sin(OS.get_ticks_msec() * multiplier) * 1.0
                */

                // 	                if OS.get_ticks_msec() - lastStepTime > 100:
                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("OS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_ticks_msec");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("lastStepTime");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(100));
                yield return new Token(TokenType.Colon);

                // 		                lastStepTime = OS.get_ticks_msec()
                // indent increase to 2
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("lastStepTime");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("OS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_ticks_msec");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);

                // 		                if Input.is_action_pressed("wag_faster"):
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("wag_faster"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                // 			                multiplier = clamp(multiplier + {stepAmount}, 0, 1)
                // indent increase to 3
                //yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("multiplier");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.LogicClamp);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("multiplier");
                yield return new Token(TokenType.OpAdd);
                // yield return new IdentifierToken("multiplier");
                yield return new ConstantToken(new RealVariant(stepAmount));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);

                // 		                if Input.is_action_pressed("wag_slower"):
                // indent decrease to 2
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("wag_slower"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                // 			                multiplier = clamp(multiplier - {stepAmount}, 0, 1)
                // indent increase to 3
                //yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("multiplier");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.LogicClamp);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("multiplier");
                yield return new Token(TokenType.OpSub);
                //yield return new IdentifierToken("multiplier");
                yield return new ConstantToken(new RealVariant(stepAmount));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);


                // 	                if wagging:
                // indent decrease to 1
                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("wagging");
                yield return new Token(TokenType.Colon);

                // 		                wag = sin(OS.get_ticks_msec() * multiplier) * 1.0
                // indent increase to 2
                //yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("wag");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.MathSin);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("OS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_ticks_msec");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpMul);
                yield return new IdentifierToken("multiplier");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(1.0));
            }
        }
    }
}
