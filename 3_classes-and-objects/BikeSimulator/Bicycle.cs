using BikeSimulator.components;

namespace BikeSimulator;

public class Bicycle(int startGear)
{
    public double Speed { get; private set; } = 0;
    public DateTime PedalCooldownExpire { get; private set; } = DateTime.Now;
    public DateTime BrakeCooldownExpire { get; private set; } = DateTime.Now;
    public bool IsBroken { get; private set; } = false;

    private Pedals _pedals = new Pedals();
    private Gears _gears = new Gears(maxGear: 7, startGear: startGear);
    private Light _frontLight = new Light("Front");
    private Light _rearLight = new Light("Rear");
    private Wheel _frontWheel = new Wheel("Front");
    private Wheel _rearWheel = new Wheel("Rear");
    private Brake _frontBrake = new Brake("Front");
    private Brake _rearBrake = new Brake("Rear");

    public int GetCurrentGear()
    {
        return _gears.CurrentGear;
    }

    public double GetOptimalSpeed()
    {
        return 5.0 * Math.Pow(1.4, _gears.CurrentGear - 1);
    }

    public void StepPedal()
    {
        if (PedalCooldownExpire > DateTime.Now)
        {
            Console.WriteLine("Pedal cooldown active");
            return;
        }

        if (!IsBikeRidable())
        {
            Console.WriteLine("Bicycle not in rideable condition");
            return;
        }

        Speed += _pedals.Step(_gears.CurrentGear, Speed);
        PedalCooldownExpire = DateTime.Now.AddMilliseconds(_calculatePedalCooldown());
    }

    public void GearShiftUp()
    {
        _gears.ShiftUp();
    }

    public void GearShiftDown()
    {
        _gears.ShiftDown();
    }

    public void Brake(bool brakeFront, bool brakeRear)
    {
        if (BrakeCooldownExpire > DateTime.Now)
        {
            Console.WriteLine("Brake cooldown active");
            return;
        }

        if (brakeFront)
        {
            if (Speed > 50 && !brakeRear)
            {
                _frontBrake.Use(Speed);
                IsBroken = true;
                Speed = 0;
                Console.WriteLine("The bike flips over, launching you into upcoming traffic resulting in your unfortunate death :c");
                return;
            }
            Speed -= _frontBrake.Use(Speed);
        }
        if (brakeRear) Speed -= _rearBrake.Use(Speed);
        Speed = Math.Max(Speed, 0); // speed cannot be negative
        BrakeCooldownExpire = DateTime.Now.AddSeconds(1);
    }

    public void PunctureFrontTire()
    {
        _frontWheel.PunctureTire();
    }

    public void PunctureRearTire()
    {
        _rearWheel.PunctureTire();
    }

    public void FixFrontTire()
    {
        _frontWheel.FixTire();
    }

    public void FixRearTire()
    {
        _rearWheel.FixTire();
    }

    public void RemoveFrontTire()
    {
        _frontWheel.RemoveWheel();
    }

    public void RemoveRearTire()
    {
        _rearWheel.RemoveWheel();
    }

    public void AttachFrontTire()
    {
        _frontWheel.AttachWheel();
    }

    public void AttachRearTire()
    {
        _rearWheel.AttachWheel();
    }

    public void ToggleFrontLight()
    {
        _frontLight.ToggleLight();
    }

    public void ToggleRearLight()
    {
        _rearLight.ToggleLight();
    }

    public bool IsBikeRidable()
    {
        if (!_rearWheel.IsAttachedToBike || !_frontWheel.IsAttachedToBike) return false;
        if (_rearWheel.HasHoleInTire() || _frontWheel.HasHoleInTire()) return false;
        return !IsBroken;
    }

    private double _calculatePedalCooldown()
    {
        const double baseCooldown = 1.0;

        // higher gear has more penalty
        var gearPenalty = (_gears.CurrentGear - 1) * 0.3;

        // higher speeds decreases cooldown, as pedaling is easier the faster you move
        var speedBonus = Speed * 0.02;

        var cooldown = baseCooldown + gearPenalty - speedBonus;
        return Math.Clamp(cooldown, 0.2, 3) * 1000;
    }
}