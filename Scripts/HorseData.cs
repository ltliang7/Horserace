
public class HorseData
{
    public string session;
    public string time;
    public string level;
    public string distance;
    public string ground;
    public string ground_type;
    public string race_name;
    public Winnerinfo[] winnerinfos;
    //public List<StudentInfo> studentInfo
    public HorseData(string session, string time, string level, string distance,
                        string ground, string ground_type, string race_name, Winnerinfo[] list)
    {
        this.session = session;
        this.time = time;
        this.level = level;
        this.distance = distance;
        this.ground = ground;
        this.ground_type = ground_type;
        this.race_name = race_name;
        this.winnerinfos = list;
    }
}
