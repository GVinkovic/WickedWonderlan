using UnityEngine;

public class PlayerStats : CharacterStats {


    [Tooltip("Predstavlja koliko je fizički jak lik. Kontrolira jačinu fizičkog napada. Oklopi i oružja mogu imati određene zahtjeve poput određenog broja bodova investiranih u status Snage kako bi ih igrač mogao nositi")]
    public Stat strength;
    [Tooltip("Predstavlja tjelesnu izdržljivost tj. maksimalni health lika. Status Konstitucije  nije izravno povezan sa statusom Snage. Lik s visokim statusom Snage može vrlo lako pobijediti neprijatelje, ali može i lako umrijeti ako mu je Konstitucija preniska")]
    public Stat constitution;
    [Tooltip("Status koji utječe na količinu mane (resurs za korištenje magije) i na regeneraciju mane (po sekundi) i jačinu magičnih napada. Također različite magije mogu imati zahtjeve poput određenog broja bodova investiranih u status Inteligencije kako bi se mogle koristiti")]
    public Stat intelligence;
    [Tooltip("Status koji povećava brzinu kretanja lika (eng. movement speed), kao i šansu kritičnog napada (eng. critical hit chance). Kritični napad je napad pri kojem se zadana šteta (eng. damage) duplicira. To je napad koji se ne događa pri svakome udarcu, već ima određenu šansu da se dogodi, a veličina te šanse ovisi o bodovima uloženim u status Spretnosti")]
    public Stat dexterity;

    public int MaxMana = 100;
    public int CurrentMana { get; set; }

    public override void Awake()
    {
        base.Awake();
        CurrentMana = MaxMana;
        MaxHealth += constitution.Value * 5;
        MaxMana += intelligence.Value * 5;
    }

    public override void TakeHit()
    {
        base.TakeHit();
        PlayerManager.TakeHit();
		if (Random.Range (0, 4) == 0) {
			gameObject.GetComponent<AudioController> ().Play ("TakeHitScream");
		}

		gameObject.GetComponent<AudioController> ().Play ("TakePunch");
    }
    public override void Die()
    {
        base.Die();
        PlayerManager.Die();
		gameObject.GetComponent<AudioController>().Play ("GameOver");
		gameObject.GetComponent<AudioController>().Play ("DeathScream");
    }

    public override bool IsAttacking()
    {
        return  PlayerManager.PlayerScript.IsAttacking;
    }
    public override int GetDamageValue()
    {
        int val = damage.Value;

        var playerType = PlayerManager.PlayerScript.Type;

        // ako trenutno napada mačem poveća damage za vrijednost snage
        if (playerType == Player.PlayerType.Sword) val += strength.Value;

        // ako trenutno napada magijom poveća damage za vrijednost inteligencije
        else if (playerType == Player.PlayerType.Magic) val += intelligence.Value;

        // sansa za dupli damage
        if (Random.Range(0, 100) <= dexterity.Value) val *= 2;
        return val;
    }

    public void AlterIntelligence(int amount)
    {
        intelligence.Value += amount;
        MaxMana += amount * 5;
    }


    public void AlterDexterity(int amount)
    {
        dexterity.Value += amount;
    }

    public void AlterConstitution(int amount)
    {
        constitution.Value += amount;
        MaxHealth += amount * 5;
    }

    public void AlterStrength(int amount)
    {
        strength.Value += amount;
    }

}
