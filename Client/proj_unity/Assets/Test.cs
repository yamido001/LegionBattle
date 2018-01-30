using UnityEngine;
using LBMath;
using System.Text;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TestSqrManitu();
	}

    void TestRandom()
    {
        MyRandom a = new MyRandom();
        a.Seed = 11111111;
        StringBuilder sb = new StringBuilder();
        int testCount = 100000000;
        int[] randomArray = new int[testCount];
        for (int i = 0; i < randomArray.Length; ++i)
        {
            randomArray[i] = a.Random();
            //sb.Append(a.Random().ToString() + "\t");
        }
        //Debug.LogError("随机数\n " + sb.ToString());

        for (int i = 1; i < randomArray.Length; ++i)
        {
            if (randomArray[0] == randomArray[i])
            {
                Debug.LogError(i.ToString());
            }
        }
    }

    void TestSqrManitu()
    {
        int testCount = 10;
        MyRandom a = new MyRandom();
        a.Seed = 1231231;
        for(int i = 0; i < testCount; ++i)
        {
            IntegerFloat intFloat = IntegerFloat.Zero;
            IntVector2 pos = new IntVector2(a.Random(0, 100), a.Random(0, 100));
            using ( new Utils.PerformanceCal("My"))
            {
                for(int j = 0; j < 10000; ++j)
                    intFloat = pos.safeMagnitude;
            }
            double systemSqr = 0;
            using (new Utils.PerformanceCal("Sy"))
            {
                for (int j = 0; j < 10000000; ++j)
                    systemSqr = System.Math.Sqrt(pos.SqrMagnitude);
            }

            Debug.LogError("我的开平方的值 " + intFloat.ToFloat() + " 系统的开平方值 " + systemSqr);
        }
    }
}
