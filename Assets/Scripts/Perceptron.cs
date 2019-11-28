using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet
{
    public double[] input;
    public double output;
}


public class Perceptron : MonoBehaviour
{
    public TrainingSet[] ts;

    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;

    public SimpleGrapher sg;

    double DotProductBias(double[] inputs, double[] weights)
    {
        if(inputs == null || weights == null) { return -1; }
        if(inputs.Length != weights.Length) { return -1; }

        double result = 0;
        for(int x = 0; x < inputs.Length; x++)
        {
            result += inputs[x] * weights[x];
        }
        result += this.bias;

        return result;
    }

    double ActivationFunction(int i)
    {
        double dp = DotProductBias(this.ts[i].input, this.weights);
        if(dp > 0) { return 1; }
        return 0;
    }

    void UpdateWeights(int j)
    {
        double error = ts[j].output - ActivationFunction(j);
        this.totalError += Mathf.Abs((float)error);
        for(int i = 0; i < this.weights.Length; i++)
        {
            // new weight = input * error + old weight;
            this.weights[i] = this.ts[j].input[i] * error + this.weights[i];
        }
        this.bias += error;
    }

    void InitialiseWeights()
    {
        for(int i = 0; i < this.weights.Length; i++)
        {
            this.weights[i] = Random.Range(-0.1f, 0.1f);
        }
        this.bias = Random.Range(-0.1f, 0.1f);
    }

    double TestOutput(double input1, double input2)
    {
        double[] inp = new double[] { input1, input2 };
        double dp = DotProductBias(this.weights, inp);
        if (dp > 0) { return 1; }
        return 0;
    }

    void Train(int epochs)
    {
        InitialiseWeights();

        for(int e = 0; e < epochs; e++)
        {
            this.totalError = 0;
            for(int t = 0; t < ts.Length; t++)
            {
                UpdateWeights(t);
                Debug.Log("W1: " + (this.weights[0]) + " W2: " + (this.weights[1]) + " B: " + bias);
            }
            Debug.Log("TOTAL ERROR: " + this.totalError);
        }
    }

    void DrawAllPoints()
    {
        for(int t = 0; t < this.ts.Length; t++)
        {
            if(this.ts[t].output == 0)
            {
                this.sg.DrawPoint((float)this.ts[t].input[0], (float)this.ts[t].input[1], Color.magenta);
            }
            else
            {
                this.sg.DrawPoint((float)this.ts[t].input[0], (float)this.ts[t].input[1], Color.green);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawAllPoints();

        Train(500);

        this.sg.DrawRay((float)(-(this.bias / this.weights[1]) / (this.bias / this.weights[0])), (float)(-this.bias / this.weights[1]), Color.red);

        if(TestOutput(0.3, 0.9) == 0)
        {
            this.sg.DrawPoint(0.3f, 0.9f, Color.red);
        }
        else
        {
            this.sg.DrawPoint(0.3f, 0.9f, Color.yellow);
        }

        if (TestOutput(0.8, 0.1) == 0)
        {
            this.sg.DrawPoint(0.8f, 0.1f, Color.red);
        }
        else
        {
            this.sg.DrawPoint(0.8f, 0.1f, Color.yellow);
        }


        Debug.Log("Test 0 0: " + TestOutput(0, 0));
        Debug.Log("Test 0 1: " + TestOutput(0, 1));
        Debug.Log("Test 1 0: " + TestOutput(1, 0));
        Debug.Log("Test 1 1: " + TestOutput(1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
