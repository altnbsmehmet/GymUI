public class ProfileViewModel : ViewModelBase
{
    public UserDto User { get; set; }
    public EmployeeDto Employee { get; set; }
    public MemberDto Member {get; set; }
    public List<SubscriptionDto> Subscriptions { get; set; }
}