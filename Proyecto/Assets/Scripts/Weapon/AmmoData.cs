using UnityEngine;

[System.Serializable]
public class AmmoData
{
    public int magazineSize;
    public int reserveAmmo;
    public int maxReserveAmmo;

    [HideInInspector]
    public int currentAmmo;

    public AmmoData(int magazineSize, int reserveAmmo)
    {
        this.magazineSize = magazineSize;
        this.reserveAmmo = reserveAmmo;
        this.maxReserveAmmo = reserveAmmo; // guardamos el valor máximo original
        this.currentAmmo = magazineSize;
    }

    public void RefillReserve()
    {
        reserveAmmo = maxReserveAmmo;
    }

    // Recarga normal o completa
    public void Reload(bool full = false)
    {
        if (full)
        {
            currentAmmo = magazineSize;
            return;
        }

        int needed = magazineSize - currentAmmo;
        int toReload = Mathf.Min(needed, reserveAmmo);
        currentAmmo += toReload;
        reserveAmmo -= toReload;
    }

    public bool CanShoot() => currentAmmo > 0;

    public void ConsumeAmmo() => currentAmmo--;

    public void ReplenishReserve(int amount)
    {
        reserveAmmo += amount;
    }


}
