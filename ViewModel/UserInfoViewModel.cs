using Data;

public class UserInfoViewModel : ViewModelBase
{
    public ApplicationUser User { get; set; }
    public Employee Employee { get; set; }
    public Member Member {get; set; }
    public List<Subscription> Subscriptions { get; set; }
}