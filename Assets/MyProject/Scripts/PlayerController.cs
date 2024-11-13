using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        // Obt�m a posi��o do mouse em rela��o ao mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcula o �ngulo entre o objeto e o mouse
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        // Define a rota��o do objeto
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
