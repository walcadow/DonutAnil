using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject stand;
    public float distanceBetween = 0.5f;
    public float startY = 1.0f;
    public Stand lastStand = null;
    public static GameManager instance;

    private void Start()
    {
        instance = this;

        // �lk Stand'lar� olu�tur
        CreateInitialStands();
    }

    private void CreateInitialStands()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                Vector3 pos = new Vector3(x * distanceBetween, startY, z * distanceBetween);
                Instantiate(stand, pos, Quaternion.identity);
            }
        }

        // Bo� donutlu stand ekle
        GameObject emptyDonut = Instantiate(stand, new Vector3(3 * distanceBetween, startY, 0 * distanceBetween), Quaternion.identity);
        emptyDonut.GetComponent<Stand>().ClearDonuts();

        // Di�er stand'lar� olu�tur
        for (int z = 1; z < 3; z++)
        {
            Vector3 pos = new Vector3(3 * distanceBetween, startY, z * distanceBetween);
            Instantiate(stand, pos, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && TryGetClickedStand(out Stand clickedStand))
        {
            HandleStandClick(clickedStand);
        }
    }

    private bool TryGetClickedStand(out Stand clickedStand)
    {
        RaycastHit hit;
        clickedStand = null;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500.0f, LayerMask.GetMask("Stand")))
        {
            clickedStand = hit.transform.gameObject.GetComponent<Stand>();
            return clickedStand != null;
        }

        return false;
    }

    private void HandleStandClick(Stand clickedStand)
    {
        // E�er �nceki ve mevcut stand ayn� ise donut durumunu de�i�tir
        if (lastStand == clickedStand)
        {
            ToggleDonutState(lastStand);
            return;
        }

        // E�er yeni t�klanan stand'ta yeterli alan varsa ve donut hareketi yapabiliyorsak
        if (lastStand != null && lastStand.donutState == DonutState.Rising && clickedStand.donuts.Count < 4)
        {
            // Di�er stand'da bir donut "Rising" ise i�lem yapma
            if (clickedStand.donutState == DonutState.Rising)
                return;

            MoveDonutBetweenStands(clickedStand);
        }

        // Son t�klanan stand'� g�ncelle
        lastStand = clickedStand;
    }

    private void ToggleDonutState(Stand stand)
    {
        if (stand.donutState == DonutState.Rising)
        {
            stand.donutState = DonutState.Idle;
        }
        else if (stand.donutState == DonutState.Idle)
        {
            stand.donutState = DonutState.Rising;
        }
    }

    private void MoveDonutBetweenStands(Stand clickedStand)
    {
        Donut donut = lastStand.donuts[^1]; // Son donut
        lastStand.donuts.RemoveAt(lastStand.donuts.Count - 1); // Sonuncu donut'� kald�r
        clickedStand.donuts.Add(donut); // Yeni stand'a ekle

        // Durumlar� g�ncelle
        clickedStand.donutState = DonutState.Rising;
        lastStand.donutState = DonutState.Idle;
    }
}
