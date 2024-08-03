using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    //[Header("Weather")]
    //[SerializeField] private Global.WeatherTypes weaterType;
    
    public void Init() {
        /*int wt = (int)weaterType;
        GameObject[] cars = ManagerGame.instance.GetCars();
        int cn = Mathf.Clamp(SaveLoad.saveData.lastCar, 0, cars.Length - 1);
        GameObject newCar = Instantiate(cars[cn], spawnPoint.position, Quaternion.identity);
        newCar.TryGetComponent<PlayerWeather>(out PlayerWeather weatherParticle);
        weatherParticle?.SetWeather(wt);*/
    }
}
