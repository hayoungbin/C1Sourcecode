using UnityEngine;

public class Hints : MonoBehaviour
{
    public void SetHints(int num)
    {
        switch (num)
        {
            case 1:
                SetHint1();
                break;
            case 2:
                SetHint2();
                break;
            default:
                break;
        }
    }

    private void SetHint1()
    {
        string str1;
        string str2;

        if (GameManager.Instance.randam == 1)
        {
            str1 = "2층 휴게실";
            str2 = "지하 끝 방";
        }
        else
        {
            str1 = "지하 끝 방";
            str2 = "2층 휴게실";
        }

        GameManager.Instance.hintMassage.text = "야간 순찰 당번에게\n" +
            "\n" +
            "퇴근하기 전에 1층 현관 옆의 관리실에 있는 차단기를 내리는 걸 잊지마.\n" +
            "그리고 1층, 2층, 지하순으로 둘러봐주고, 차단기가 잘 작동하는지도 확인해주고\n" +
            $"{str1}에 있는 차단기도 확실히 내려줘\n" +
            $"아, 그리고 {str2}에 있는 차단기는 왠만하면 건드리지마\n" +
            "왜인지는 나도 몰라, 그냥 여기 주인장이 그러던데\n" +
            "무슨 일이 있어도 절대로 건드리지 말라고 그러더라\n" +
            "그럴거면 그냥 없애는게 낫지 않나 싶기도 한데...";
    }
    private void SetHint2()
    {
        GameManager.Instance.hintMassage.text = $"X X X {GameManager.Instance.passward4}";
    }
}