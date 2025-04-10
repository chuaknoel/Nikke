using UnityEngine;

public class MakeDororong : MonoBehaviour
{
    public GameObject dororong;
    public GameObject dororong1;
    public GameObject dororong2;
    public GameObject dororong3;
    public GameObject dororong4;
    public GameObject dororong5;




    void Start()
    {
        InvokeRepeating("Makedoro", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Makedoro()
    {
        Instantiate(dororong);
        Instantiate(dororong1);
        Instantiate(dororong2);
        Instantiate(dororong3);
        Instantiate(dororong4);
        Instantiate(dororong5);
    }

}
