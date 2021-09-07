using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public static DamageNumbers instance;
    public static DamageNumbers DamageInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DamageNumbers>();
            }
            return instance;
        }

    }
    public float moveSpeed;
    public int damageNumber;
    public TextMeshProUGUI displayNumber;
    public bool isRedAttack = false;
    public bool isMiss = false;

    void Update()
    {

        if (isRedAttack)
        {
            displayNumber.color = Color.red;
            displayNumber.fontSize = 1.75f;
            displayNumber.fontStyle = FontStyles.Bold;
            displayNumber.fontStyle = FontStyles.Italic;
            displayNumber.text = "" + damageNumber;
            transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), transform.position.z);
        }
        else if(isMiss)
		{
            displayNumber.color = Color.yellow;
            displayNumber.fontSize = 1f;
            displayNumber.text = "Miss";
            transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), transform.position.z);
		}
        else
        {
            displayNumber.text = "" + damageNumber;
            transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), transform.position.z);
        }
    }
    
}