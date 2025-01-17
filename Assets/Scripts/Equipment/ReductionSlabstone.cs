using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReductionSlabstone : MonoBehaviour
{
    public int reductionNumber;

    
    public ReductionSlabstone(int number)
    {
        this.reductionNumber = number;
    }
    public int GetReductionNumber()
    {
        return this.reductionNumber;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<SlabStoneContainer>().AddSlabStone(this);
            gameObject.SetActive(false);
        }
    }
}
