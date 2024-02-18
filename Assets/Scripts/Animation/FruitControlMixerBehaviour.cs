using UnityEngine;
using UnityEngine.Playables;

public class FruitControlMixerBehaviour : PlayableBehaviour
{
    public GameObject fruit = null;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //Light trackBinding = playerData as Light;
        //float finalIntensity = 0f;
        //Color finalColor = Color.black;
        Vector3 finalPos = Vector3.zero;
        bool useLocal = false;

        if (fruit != null)
        {
            float lastWeight = 0; ;
            int inputCount = playable.GetInputCount(); //get the number of all clips on this track

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<FruitControlBehaviour>)playable.GetInput(i);
                FruitControlBehaviour input = inputPlayable.GetBehaviour();

                // Use the above variables to process each frame of this playable.
                //finalIntensity += input.intensity * inputWeight;
                //finalColor += input.color * inputWeight;

                finalPos += inputWeight * input.endPos;

                if(inputWeight > lastWeight)
                {
                    useLocal = input.useLocal;
                    lastWeight = inputWeight;
                }
            }

            ////assign the result to the bound object
            //trackBinding.intensity = finalIntensity;
            //trackBinding.color = finalColor;

            if(useLocal) fruit.transform.localPosition = finalPos;
            else fruit.transform.position = finalPos;
        }
        else
        {
            fruit = playerData as GameObject;
        }
    }
}