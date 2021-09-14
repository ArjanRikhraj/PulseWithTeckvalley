using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pulse
{
    public static partial class Constant
    {
        #region API Url's
        //public const string BaseServiceUrl_local = "https://localhost:8000";
        public const string BaseServiceUrl_local = "http://10.0.2.2:8000";
        public const string BaseServiceUrl_testing = "http://pulseph2-qc.netsol.in";
        public const string BaseServiceUrl_staging = "http://pli-pulseph2.agilecollab.com";
        // public const string BaseServiceUrlLive = "https://pulseapp.ca";
        public const string BaseServiceUrlLive = "https://prod.pulseapp.ca";
       // public const string BaseServiceUrlLive = "https://35.182.22.214";

        //public const string BaseServiceUrl = "http://pli-socialma.agilecollab.com";
        public const string BaseServiceUrl = BaseServiceUrlLive;
        public const string TokenServiceUrl = BaseServiceUrlLive + "/o/token/";
        public const string SignUpUrl = "/api/accounts/signup/";
        public const string SocialSignUpUrl = "/api/accounts/sociallogin/";
        public const string SignInUrl = "/api/accounts/signin/";
        public const string SignOutUrl = "/api/accounts/signout/";
        public const string CheckEmailExistUrl = "/api/accounts/checkemail/";
        public const string SendOtpOnEmailUrl = "/api/accounts/verifyemail/";
        public const string SendOtpOnMobileUrl = "/api/accounts/verifymobile/";
        public const string VerifyTokenUrl = "/api/accounts/verifytoken/";
        public const string ForgotPasswordUrl = "/api/accounts/forgotpassword/";
        public const string ResetPasswordUrl = "/api/accounts/resetpassword/";
        public const string CheckSocialUserExistUrl = "/api/accounts/checkesocialuser/";
        public const string AwsDetailUrl = "/api/accounts/app-config/";
        public const string GetUserDetail = "/api/accounts/detailsuser/";
        public const string SearchUserUrl = "/api/accounts/searchuser/?page=";
        public const string SearchFriendUrl = "/api/accounts/searchfriends/?page=";
        public const string SearchPulseUserUrl = "/api/accounts/searchpulseuser/?page=";

        public const string SearchString = "&search_term=";
        public const string AddFriendUrl = "/api/accounts/addfriends/";
        public const string RequestChangeUrl = "/api/accounts/requeststatuschange/";
        public const string MyFriendsListUrl = "/api/accounts/myfriends/?page=";
        public const string PendingFriendsListUrl = "/api/accounts/pendingrequestlist/?page=";
        public const string TimeZoneUrl = "/api/accounts/time-zones/";
        public const string PendingRequestCountUrl = "/api/accounts/pendingrequestcount/";
        public const string CreateEventUrl = "/api/event/event/";
        public const string StarEventUrl = "/api/event/starevent/";
        public const string PinStoryUrl = "/api/event/pinstory/";
        public const string AllUpcomingUrl = "/api/event/allupcomingevents/?page=";
        public const string AllPastUrl = "/api/event/allpastevents/?page=";
        public const string InterestedUpcomingUrl = "/api/event/interestedupcomingevents/?page=";
        public const string CheckedInUpcomingUrl = "/api/event/checkedin_upcomingevents/?page=";
        public const string HostedUpcomingUrl = "/api/event/hostedupcomingevents/?page=";
        public const string AttendingUpcomingUrl = "/api/event/attendingupcomingevents/?page=";
        public const string HostedPastUrl = "/api/event/hostedpastevents/?page=";
        public const string AttendedPastUrl = "/api/event/attendingpastevents/?page=";
        public const string CheckedInPastUrl = "/api/event/checkedin_pastevents/?page=";
        public const string NextSevenDaysEventUrl = "/api/event/nexts-even-days-events/?page=";
        public const string MyEventsUrl = "/api/event/myevent/";
        public const string UpdateEventUrl = "/api/event/updateevent/";
        public const string CancelEventUrl = "/api/event/cancelevent/";
        public const string LikeEventUrl = "/api/event/likeevent/";
        public const string SetCommentUrl = "/api/event/seteventcomment/";
        public const string GetAllCommentsUrl = "/api/event/geteventcomments/";
        public const string DeleteCommenturl = "/api/event/commentdelete/";
        public const string EventDetailsUrl = "/api/event/eventdetails/";
        public const string EventAttendingConfirmationUrl = "/api/event/attendeventconfirmation/";
        public const string AttendeesGuestUrl = "/api/event/attedees/";
        public const string InvitedGuestUrl = "/api/event/invited/";
        public const string InterestedGuestUrl = "/api/event/interested/";
        public const string ReportEventUrl = "/api/event/event_report_spam/";
        public const string ReportEventCommentUrl = "/api/event/comment_report_spam/";
        public const string NotInterestedGuestUrl = "/api/event/not-interested/";
        public const string CheckedInGuestUrl = "/api/event/checkedin/";
        public const string EventMediaListUrl = "/api/event/eventmediadetails/";
        public const string EventLiveMediaListUrl = "/api/event/livemediadetails/";
        public const string ProfileDetailUrl = "/api/accounts/friendsdetails/";
        public const string LatLongBasedEventsUrl = "/api/event/locationbasedevents/?page={0}&lat={1}&long={2}&datetime={3}";
        public const string PopularityBasedEventsUrl = "/api/event/popularitybasedevents/?page={0}&lat={1}&long={2}&datetime={3}";
        //public const string MapLatLongBasedEventsUrl = "/api/event/map_locationbasedevents/?page={0}&lat={1}&long={2}&datetime={3}";
        //public const string MapPopularityBasedEventsUrl = "/api/event/map_popularitybasedevents/?page={0}&lat={1}&long={2}&datetime={3}";
        public const string MapLatLongBasedEventsUrl = "/api/event/map_locationbasedevents/?page={0}&lat={1}&long={2}&datetime={3}";
        public const string MapPopularityBasedEventsUrl = "/api/event/map_popularitybasedevents/?page={0}&lat={1}&long={2}&datetime={3}";
        public const string SubmitQueryUrl = "/api/accounts/contactus/";
        public const string ContactUsUrl = "/api/job/contactinfo/";
        public const string TagFriendsUrl = "/api/accounts/friendslisttotag/";
        public const string FriendsPublicHostedEventUrl = "/api/event/public-hosted/";
        public const string MyProfileDetailUrl = "/api/accounts/myprofiledetails/";
        public const string UpdateUserUrl = "/api/accounts/updateuser/";
        public const string PaymentSuccessfulApi = "/api/event/payment/";
        public const string GetCountriesurl = "/api/master/countries/";
        public const string TransactionEmailVerifyUrl = "/api/event/verifyemail/";
        public const string CheckInUrl = "/api/event/checkin/";
        public const string UploadLiveMedia = "/api/event/uploadmedia/";
        public const string AddEventStories = "/api/event/addeventstory/";
        public const string GetEventStories = "/api/event/geteventstory/";
        public const string SaveEventStories = "/api/event/savestorymedia/";
        public const string ReportEventStories = "/api/event/reportstory/";
        public const string DeleteEventStories = "/api/event/deletestory/";
        public const string ReportUser = "/api/accounts/userreport/";
        public const string ReportMedia = "/api/event/reporteventmedia/";
        public const string DeleteEventMedia = "/api/event/deleteeventmedia/";
        public const string UploadEventCoverImage = "/api/event/addeventcoverphoto/";
        public const string ShareEventUrl = "/api/event/share/";
        public const string MediaDeleteUrl = "/api/event/mediadelete/";
        public const string CreatePulseUrl = "/api/group/pulse/";
        public const string GetPulseListUrl = "/api/group/allgroups/?page=";
        public const string SearchEventForPulseUrl = "/api/event/searchevent/?page=";
        public const string MessageListUrl = "/api/group/messagelist/";
        public const string UserMediaListingUrl = "/api/accounts/medialisting/?page=";
        public const string UserMediaUrl = "/api/accounts/usermedia/?page=";
        public const string GetFriendMediaList = "/api/event/getfriendmedia/?user_id={0}&page={1}";
        public const string SearchPulseUrl = "/api/group/groupsbysearch/?page=";
        public const string PulseDetailUrl = "/api/group/pulsedetail/";
        public const string PulseUpdateUrl = "/api/group/pulseupdate/";
        public const string AddMembersToPulseUrl = "/api/group/addmemberlist/";
        public const string PulseStatusChangeUrl = "/api/group/pulseinvitation/";
        public const string ClearPulseHistoryUrl = "/api/group/managehistory/";
        public const string PulseMemberOrNotUrl = "/api/group/memberornot/";
        public const string BlockUnBlockUserUrl = "/api/group/blockuser/";
        public const string AboutPulseUrl = BaseServiceUrl + "/api/job/staicpagescontent/about";
        public const string PrivacyPolicyUrl = BaseServiceUrl + "/api/job/staicpagescontent/privacy";
        public const string FAQsUrl = BaseServiceUrl + "/api/job/staicpagescontent/faq";
        public const string ChangePasswordUrl = "/api/accounts/password_change/";
        public const string PostMessageUrl = "/api/group/messagepost/";
        public const string MarkInappropriateUrl = "/api/event/mediainappropriate/";
        public const string TermsUrl = BaseServiceUrl + "/api/job/staicpagescontent/terms-and-conditions";
        public const string GetNotificationsUrl = "/api/notifications/?page=";
        public const string MarkReadNotificationsUrl = "/api/notifications/markread/";
        public const string UnreadNotificationsUrl = "/api/notifications/unreadcount/";
        public const string LatLongMapEventsUrl = "/api/event/lat_long_base/?page={0}&lat={1}&long={2}&datetime={3}&filter='{4}'";
        public const string PostUserLocationUrl = "/api/accounts/user/location/";
        public const string GetAllUsersUrl = "/api/accounts/allusers/?longitude={0}&latitude={1}";
        public const string GetAllPulseUserUrl = "/api/accounts/alluserslist/";
        public const string GetUserCreditCardDetailsUrl = "/api/accounts/carddetails/";
        #endregion

        public const string JsonContent = "application/json";
        public const string TokenTypeBearer = "Bearer";
        public const string ParaKeyClientId = "client_id";
        public const string ParaKeyClientSecret = "client_secret";
        public const string ParaKeyGrantType = "grant_type";
        public const string ParaKeyUsername = "username";
        public const string ParaKeyPassword = "password";
        public const string CmsUrl = ServiceBaseUrl + "Index.html#/contentPage/";
        public const string ServiceBaseUrl = "http://p2lapi-mobileap-stg.netsolutions.in/";
        public const string DataHostName = "www.pulseapp.com";
        public const string UpdateAppMessage = "You should update the app to use it.";
        public const string ConfirmationHeader = "Confirmation?";
        public const string PulseTapHereText = "Tap here for group info";

        #region AppString
        public const string Composable = "Composable";
        public const string WhatIsNew = "What's New?";
        public const string Upadte = "UPDATE";
        public const string Version = "Version:";
        public const string NetworkNotificationHeader = "Connection Problem";
        public const string NetworkDisabled = "There was a problem connecting. Please try again";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Ok = "OK";
        public const string Home = "Home";
        public const string UpperCaseLogin = "SIGN IN";
        public const string UpperCaseChangePassword = "CHANGE PASSWORD";
        public const string UpperCaseSignUp = "SIGN UP";
        public const string LogInText = "Sign In";
        public const string ChangePassword = "Change Password";
        public const string NoAccount = "Don't have an account?";
        public const string Account = "Already have an account?";
        public const string EmailNotRegistered = "Email is not registered with us";
        public const string ConfirmPasswordNotMatching = "Password and Confirm Password does not match";
        public const string EmailPassword = "Email or Password incorrect. Please try again";
        public const string PasswordMatchErrorMessage = "Both passwords do not match";
        public const string OldPasswordMatchErrorMessage = "Old password does not match";
        public const string RequiredFieldsMessage = "All fields are required";
        public const string InvalidMobileMessage = "Invalid Mobile No.";
        public const string InvalidCountryCodeMessage = "Invalid Country Code";
        public const string NoResultFound = "No Result found";
        public const int SearchCharacterLimit = 3;
        public const string UserExists = "User with this email already exists";
        public const string UserNotExists = "User with this email does not exists";
        public const string DeleteCommentText = "Do you want to delete this comment?";
        public const string RemoveParticipantText = "Are you sure? By this, member will be permanently removed from this group";

        public const string DeleteMediaText = "Do you want to delete this media file?";
        public const string DeleteButtonText = "Delete";
        public const string CancelButtonText = "Cancel";
        public const string CloseButtonText = "Close";
        public const string GroupText = "Group";
        public const string EventText = "Event";
        public const string ShareText = "Share";
        public const string ReportEventText = "Report Event";
        public const string ReportMediaText = "Report Media";


        public const string MaxPasswordLengthMessage = "Passwords must be of minimum length 6 and should contain a number";
        public const string InValidMobileNumber = "Invalid Mobile Number";
        public const string ReqFieldCommonMessage = "This field is required";
        public const string ApiResponseError = "Not able to fetch data from server.";
        public const string ServerNotRunningMessage = "Some trouble while syncing! Please try again.";
        public const string NoFriendsFound = "No friend found in your friendlist";
        public const string CommentPostedSuccessfully = "Comment posted successfully";
        public const string CommentRequiredMessage = "Please enter your comment first";
        public const string User_Exists = "User with this email already exists";
        public const string AlertTitleDefaultText = "Oops!";
        public const string AlertTitle = "Info";
        public const string MessgeTitle = "Message";
        public const string ContactPermissionMessage = "Allow contacts permission to see your contacts";
        public const string Done = "OK";
        public const string Success = "Success";
        public const string OTP = "One Time Password";
        public const string EnterOTP = "Enter your OTP";
        public const string Verify = "VERIFY";
        public const string Email = "E-mail*";
        public const string Name = "Name*";
        public const string Mobile = "Mobile";
        public const string Password = "Password*";
        public const string ResetPassword = "RESET PASSWORD";
        public const string ChangePasswordTitle = "Change Password";
        public const string DiscoverTabText = "DISCOVER";
        public const string CreateTabText = "CREATE";
        public const string GroupsTabText = "GROUPS";
        public const string TribesTabText = "MY TRIBES";
        //public const string ProfileTabText = "PROFILE";
        public const string OtpScreenHeaderText = "Verify & Login";
        public const string LoginUsingOtpText = "Log in using the OTP sent to";
        public const string EnterOtpText = "Enter 4 digit OTP";
        public const string NotGetCodeText = "Didn't get the code?";
        public const string VerifyText = "Verify";
        public const string ResendOtpText = "Resend OTP";
        public const string Android = "Android";
        public const string IOS = "iOS";

        public const string BorderWidth = "BorderWidth";
        public const string BorderRadius = "BorderRadius";
        public const string NotAllowedText = "Not Allowed";
        public const string GalleryAccessMessage = "Not allowed to access gallery.";
        public const string NoCameraText = "No Camera";
        public const string CameraNotAvalaibleMessage = "No camera available.";
        public const string NotMoreThanOneVideo = "You can not add more than one video";
        public const string RSVPText = "RSVP";
        public const string SaveText = "Save";
        public const string FcmToken = "FCMToken";
        public const string UnableToSyncMessages = "Unable to sync messages.";
        public const string ReportEventTitle = "Give feedback to this event.";
        public const string CancelText = "Cancel";
        public const string ViolenceText = "Violence";
        public const string NudityText = "Nudity";
        public const string HarassTxt = "Harassment";
        public const string SpamText = "Spam";
        public const string OtherText = "Other";
        public const string BlockText = "Block";
        public const string UnBlockText = "Unblock";
        public const string ReportMessage = "Admin has been notified about the issue you reported and an early action will be taken by him";
        public const string ReportDescriptionMessage = "\nPlease describe your issue in comment box it will be easy for us to understand";

        #region Social Login
        public const string FacebookText = "Facebook";
        public const string TwitterText = "Twitter";
        public const string GoogleText = "Google";
        public const string LinkedInText = "LinkedIn";
        public const string AuthenticationFailedText = "Authentication failed please try after sometime.";
        #endregion


        #endregion

        #region AppLinks
        public const string GooglePlayLink = "https://play.google.com/store?hl=en";
        public const string AppStoreLink = "https://itunes.apple.com/in/genre/music/id34";
        #endregion

        #region Regex
        public const string EmailRegEx = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[A-Za-z0-9][\-A-Za-z0-9]{0,22}[A-Za-z0-9]))$";
        public const string MobileNumberRegEx = @"^(0|[1-9][0-9]*)$";
        public const string PasswordRegEx = @"^(.*[0-9].*)$";
        public const string NumericRegEx = "^[0-9]*$";
        //public const string DecimalRegEx = "/^(0|[1-9]\\d*)(\\.\\d+)?$/";
        public const string DecimalRegEx = "[0-9]{1,5}(\\.[0-9]{1,2})?\n";
        #endregion

        //public const string PublicClientId_testing = "a7eq4tr5F5pP5VP3tJ1WeuYRJyyripnbjgQSnF5H";
        //public const string PublicClientSecret_testing = "zApLhGrZcX1ErAmejZAJbCcnPhW93KtEKERQlqq55cxfDAYwAMmJvPs9P1HT4Osfky1NTbt8DH6qsh47HHmCCqu7RPuvE4F3qo60LZDDhPmBC381zHz6xp4ig4FrtJRr";
        public const string PublicClientId_testing = "iTsQllQYEQa3OYqZ9vAYffbiSFo9KtJV2llRqQRJ";
        public const string PublicClientSecret_testing = "nb0ahzPjOZVoi7o4DGVPmh0fTdtMyEAPsGOg18Co5fyBYMKKkrwZewnsVHP4q1U4nUURzKeJoOCgzTpqqKhGdW1rCVuJFLGeaNOL1UtvkPNz92a5DsCZVyEST9Ub1aOG";

        public const string PublicClientId_staging = "opBaAREhjqhUMwk2Kwk3oYpZqf6oVfjS5vpIny3Z";
        public const string PublicClientSecret_staging = "WtIeRDkvGaoO38RpJlT2dZE6Hv7nBk5YmDDGFu52UuIH1lGyp1GRHHmkpHNeENEbiCd7z74tPeOohI80Ht6vU14NIfIe0nJm3X9SlQYQ4NnYvuFwhwtb80HO5OIsKUWD";
        public const string PublicClientId_local = "1234";
        public const string PublicClientSecret_local = "56789";
        public const string PublicClientId = "PHbauFe6GizObwCEZLRbCQZfsYjzNIn0o7JDHgQC";
        public const string PublicClientSecret = "es0EqST1u7YUiTkq7mZSYYKj0FKzCUILtCrjHS9LQFnhxcEml11DdCN6jxAT2USpsnZ7DhmBLodPXW2Hlqadpfn6hkz3xqvgXb4A1922mwAgY9zNk1D3gz7OHi3BDZHN";

        public const string UserNameForToken = "appuser@pulse.com";
        //public const string PasswordForToken_localNtesting = "Admin@123";
        public const string PasswordForToken_localNtesting = "pulse@#123";
        public const string PasswordForToken = "pulse20$#18";

        public const string PublicGrant_type = "password";
        public static readonly string GoogleApiKey = Device.RuntimePlatform == Device.iOS ? "AIzaSyB0oeyzGWhgXm_nXlRPSI-r8SqJ4b6Ybyw" : "AIzaSyBEy7WS2GwNSGsulwpmKw1apTojPLKAALA";
        public const string ProjectCode = "22839275105";
        public const double NavigationHeaderHeight = 40;
        public const double NavigationHeaderFontSize = 18;

        public const string SOCIAL_LOGIN_ERROR_MESSAGE = "oops something is wrong!";

        #region ContactUs Page
        public const string RegistrationPageFormPadding = "10";
        #endregion




        #region Pulse new header values
        public const string EmailText = "Email";
        public const string MobileText = "Mobile(with country code)";
        public const string NameText = "Name";
        public const string UserNameText = "Your Username";
        public const string UsernameText = "Username";
        public const string DobText = "Date of Birth";
        public const string SchoolText = "School/Occupation";
        public const string PasswordText = "Password";
        public const string ForgetPasswordText = "Forgot Password?";
        public const string Register = "Register";
        public const string SignInText = "SIGN IN";
        public const string SignUpText = "SIGN UP";
        public const string NoAccountText = "Don't have an account?";
        public const string SignInFacebookText = "Sign In with";
        public const string SignUpFacebookText = "Sign Up with";
        public const string SignInSmallLetter = "Sign in";
        public const string SignUpSmallLetter = "Sign up";
        public const string FacebookLoginText = "Facebook";
        public const string GoogleLoginText = "Google";
        public const string AlreadyAccountText = "Already have an account?";
        public const string TermsConditionText = "By checking this box I agree that I have read and accept";
        public const string TermsNConditionText = "'Terms & Condition'";
        public const string AndText = " & ";
        public const string PrivacyPolicyQuoteText = "'Privacy Policy'";
        //public const string TermsConditionText = "By checking this box I agree that I have read and accept 'Terms & Condition' & 'Privacy Policy'";
        public const string VerifyEmailText = "Verify Email Address";
        public const string VerifyMobileText = "Verify Mobile Number";
        public const string VerifyMobileDescription = "We have sent an OTP to your mobile number";
        public const string EmailForOTP = "Jason@netsolutions.com";
        public const string EnterCodeText = "Please enter the code here to validate";
        public const string NoOTPText = "Didn't receive an OTP?";
        public const string SendOTPText = "Send Again";
        public const double OTPTextSize = 20;
        public const string VerifyMessage = "Verification Successful";
        public const string AccountCreated = "Your Pulse account has been created";
        public const string AllSetText = "You're all Set!";
        public const string UploadPicText1 = "Take a minute to upload a profile picture so";
        public const string UploadPicText2 = "your friends can discover you!";
        public const string TroubleSigningIn = "Having trouble signing in?";
        public const string ForgotEmailMessage = "Please enter your registered Email ID to";
        public const string ForgotEmailRecieveMessage = "receive password reset instructions";
        public const string SubmitButtonText = "SUBMIT";
        public const string NewPasswordText = "New Password";
        public const string ConfirmPasswordText = "Confirm New Password";
        public const string ForgotPassword = "Forgot Password";
        public const string ChooseProfileText = "Choose Profile Picture";
        public const string TakePhotoText = "Take Photo";
        public const string ChoosePhotoText = "Choose Photo";
        public const string OtpResentText = "OTP resent to you";
        public const string PasswordResetSuccessfully = "Password reset successfully";
        public const string RegisteredSuccessfully = "You are registered successfully";
        #endregion
        #region validation strings
        public const string EnterEmail = "Email address is required";
        public const string EnterPassword = "Password is required";
        public const string EnterNewPassword = "New password is required";
        public const string OldPasswordRequired = "Old password is required";
        public const string EnterConfrimPassword = "Confirm password is required";
        public const string NameRequired = "Name is required";
        public const string EmailNotValid = "Invalid email id";
        public const string PasswordLengthMessage = "Password should be atleast 6 and less than 15 characters";
        public const string PasswordAlphanumericMessage = "Password should be alphanumeric";
        public const string TermsAccpetText = "Please accept terms and conditions";
        public const string InvalidCredentialsMessage = "Invalid Credentials. Please try again";
        public const string UserNameRequired = "Username is required";
        public const string CountryCodeRequired = "Please enter country code";
        public const string MobileRequired = "Please enter mobile number";
        public const string ConfimText = "Confirm";
        public const string DeleteText = "Delete";
        public const string ReportText = "Report";
        public const string EndEventText = "End";
        public const string SaveStoryText = "Save Story";
        public const string DeleteStoryText = "Delete Story";
        public const string ReportStoryText = "Report Story";
        public const string PastDateCannotSelectMessage = "Past date cannnot be selected";
        public const string EndDateGreaterThenStartDateMessage = "End Date should be after start date or on same date";
        public const string PastTimeCannotSelectMessage = "Past time cannnot be selected";
        public const string EndTimeGreaterThenStartTimeMessage = "End time should be after start time for same date event";
        public const string VideoValidation = "Video size should be less than 30MB.";
        public const int VideoValidationMB = 30;
        public const string CanNotAddMore = "Sorry,you can only add upto five photos and one video";
        #endregion
        #region AppFooter strings
        public const string EventTabText = "Events";
        public const string PulseTabText = "Pulse";
        public const string NotificationTabText = "Notification";
        public const string FriendsTabText = "Friends";
        public const string ProfileTabText = "Profile";
        #endregion

        #region Status Codes
        public const int StatusZero = 0;
        public const int Status200 = 200;
        public const int Status111 = 111;
        public const int Status400 = 400;
        public const int Status401 = 401;
        #endregion

        #region Friends
        public const string AddFriendText = "Add Friend";
        public const string FriendText = "Friends";
        public const string CancelRequestText = "Cancel Request";
        public const string FriendRequestText = "Friend Request";
        public const string ConfirmRequestText = "Confirm Request";
        public const string SearchText = "Search";
        public const string PendingText = "Pending";
        #endregion

        #region AddEvent 
        public const string EventTitle = "Event Title";
        public const string MobieNumber = "Mobile Number";
        public const string VenueTitle = "Venue";
        public const string AddressText = "Address";
        public const string UnitText = "Unit";

        public const string CityText = "City";
        public const string StateText = "State";
        public const string ZipCodeText = "Postal Code";
        public const string CountryText = "Country";
        public const string FromText = "From DateTime";
        public const string ToText = "To DateTime";
        public const string TimeText = "Time";
        public const string GuestText = "Guests";
        public const string AddGuestText = "+ Add Friends and Contacts";
        public const string PhotosText = "Cover photo & Event Media";
        public const string AddPhotosText = "+ Add Photos or Videos";
        public const string CreateEventText = "Create Event";
        public const string NextButtonText = "Next";
        public const string PreviousButtonText = "Previous";
        public const string EditEventText = "Update Event";
        public const string MoreText = "More";
        public const string LessText = "Less";
        public const string UnderDevelopment = "Under Development";
        public const string EventCreated = "Event created successfully";
        public const string EventUpdated = "Event updated successfully";
        public const string UserUpdated = "User details are updated successfully";
        public const string EventCancelled = "Event cancelled successfully";
        public const string CommentDeleted = "Comment deleted successfully";
        public const string MediaDeleted = "Media file deleted successfuly";

        public const string EventTitleRequired = "Event title is required";
        public const string EventVenueRequired = "Event venue is required";
        public const string EventAddressRequired = "Event unit is required";
        public const string MobileNumberRequired = "Mobile number is required";
        public const string ValidMobileNumberRequired = "Enter valid mobile number";
        public const string BottleAmountRequired = "Please input per bottle fee";
        public const string CoverAmountRequired = "Please input cover fee";
        public const string CoverAmountValid = "Please input valid cover fee";
        public const string BottleAmountValid = "Please input valid bottle fee";
        public const string GuestsRequired = "Please add guests in case of private event";
        public const string TimeZoneText = "Timezone";
        public const string AddEventsPhotosText = "Add Event Photos/Videos";
        public const string CameraText = "Take Photo/Video";
        public const string GalleryText = "Choose from Gallery";
        public const string PhotoText = "Photos";
        public const string VideoText = "Videos";
        public const string AddMoreText = "+ Add More";
        public const string DescriptionText = "Description";
        public const string UpcomingTabText = "Upcoming Events";
        public const string PastTabText = "Past Events";
        public const string FilterText = "Filter";
        public const string AllText = "All";
        public const string HostingText = "Hosting";
        public const string AttendingText = "Guest list";
        public const string HostedText = "Hosted";
        public const string AttendedText = "Attended";

        public const string InterestedText = "Interested";
       // public const string CheckedInText = "Checked In";

        public const string PickerDoneClickText = "Done";
        public const string EditText = "Edit";
        public const string DeleteQuestionText = "Are you sure you want to cancel this event?";
        public const string MentionReasonToCancel = "Please mention reason for cancelling event";
        #endregion

        #region EventDetails
        public const string LitScoreText = " Lit Score";
        public const string EventDescriptionText = "Event Description";
        public const string LikesText = " Likes";
        public const string LikeText = " Like";
        public const string CommentsText = "Comments";
        public const string WriteCommentText = "Write a comment..";
        public const string ViewAllText = "View All";
        public const string GuestInvitedText = "Guests Invited";
        public const string NotInterestedText = "Not Interested";
        public const string PaidEventText = "This is a paid event with $";
        public const string CoverFeeText = " Cover Fee.";
        public const string JoinText = "Pay for event";
        public const string CheckInText = "Check-In";
        public const string CheckedIn = "Checked-In";
        public const string CheckedInText = "✔ Checked-In";
        public const string ViewTicketText = "✔ View Ticket";
        public const string JoinGuestListText = "Join Guest List";
        public const string JoinedGuestListText = "✔ Guest List";
        public const string InvitedText = "Invited";
        public const string CheckInInfoText = "Check-In to upload event pictures & videos";
        public const string SuccessCheckInInfoText = "Add event pictures & videos to the party story";


        #endregion

        #region WebView Page
        public const string AboutUsText = "About Us";
        public const string ContactUsText = "Contact Us";
        public const string PrivacyPolicyText = "Privacy Policy";
        public const string AboutPulseText = "About Pulse";
        public const string PrivayPolicyText = "Privacy Policy";
        public const string FaqsText = "FAQ's";
        public const string AboutText = "ABOUT";
        public const string HaveAQueryText = "Have a Query?";
        public const string AskUs = "Ask Us";
        public const string MailUs = "Mail Us";
        public const string CustomerSupportNumber = "Customer Support Number";
        public const string Or = "or";
        public const string ChangePasswordEvent = "changepassword";
        public const string AboutPulseEvent = "aboutpulse";
        public const string PrivacyPolicyEvent = "privacypolicy";
        public const string FaqsEvent = "faqs";
        public const string ContactUsEvent = "contactus";
        public const int DelayValue = 1000;
        public const string IsLoading = "IsLoading";
        public const string HaveAQueryMessage = "Please enter the details of your request. A member of our support staff will respond as soon as possible.";
        public const string YourEmailAddress = "Your Email Address";
        public const string DescribeYourQuery = "Describe Your Query";
        public const string SubmitRequestText = "Submit Request";
        public const string OldPasswordText = "Old Password";
        #endregion


        #region Payment
        public const string CreditCardText = "Credit/ Debit Card Number";
        public const string NameOnCardText = "Name on Card";
        public const int ExpiryYearStart = 2019;
        public const int ExpriryYearRange = 25;
        #endregion

        #region Credit Card Type Regex
        public const string VisaCardRegex = @"^4[0-9]{12}(?:[0-9]{3})?$";
        public const string MasterCardRegex = @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$";
        public const string AmericanExpressCardRegex = @"^3[47][0-9]{13}$";
        public const string DiscoverCardRegex = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
        public const string InvalidStringRegex = @"[<>]";
        public const string AlphaNumericRegex = @"^[a-zA-Z0-9]+$";
        public const string InvalidNumericRegex = @"^[0-9]*$";
        #endregion
        #region Credit Card Types
        public const string VisaCardType = "visa";
        public const string MasterCardType = "mastercard";
        public const string AmericanExpressCardType = "amex";
        public const string DiscoverCardType = "discover";
        #endregion
        #region Payment Validation Messages
        public const string CardNumberRequired = "Card number is required";
        public const string CardNumberLength = "Card number length should be 16";
        public const string InvalidCardNumber = "Invalid card number";
        public const string CardTypeNotSupported = "Invalid card type";
        public const string ExpiryMonthRequired = "Please select expiry month";
        public const string ExpiryYearRequired = "Please select expiry year";
        public const string CVVRequired = "CVV is required";
        public const string InvalidCVV = "Invalid CVV";
        public const string NameOnCardReuired = "Please mention name on card";
        public const string InvalidNameOnCard = "Invalid name on card";
        public const string CountryRequired = "Country is required";
        public const string BillingAddressReuired = "Address is required";
        public const string BillingCityReuired = "City is required";
        public const string BillingZipReuired = "Postal Code is required";
        public const string InvalidAddress = "Invalid address format";
        public const string InvalidCity = "Invalid city format";
        public const string InvalidZip = "Invalid postal code";


        public const string PaymentDeclinedMessage = "Payment has been declined";
        public const string PaymentSuccessfulMessage = "Payment successfully done";
        #endregion
        #region Payment Page Texts
        public const string CurrencyType = "CAD";
        public const string PaymentIntent = "sale";
        public const string PaymentMethod = "credit_card";
        public const string TransactionDescriptionText = "This is the payment transaction description";
        #endregion
        #region Event Check In Texts
        public const string CheckInNotProperMessage = "It seems you still haven't reached the event destination or location services are disabled on your device. Please try again later.";
        public const string CheckInEarlyMessage = "The event you're trying to check-in to hasn't started yet. Please try again later once the event has started & you have reached the destination.";
        public const string CheckInEarlyTitleMessage = "Too Early to Check-In";
        public const string CheckInNotProperTitleMessage = "Check-In Failed";
        public const string AddMediaNotProperMessage = "It seems you still haven't reached the event destination or location services are disabled on your device. Please try again later.";
        public const string AddMediaEarlyMessage = "The event you're trying to add photos/videos to hasn't started yet. Please try again later once the event has started & you have reached the destination.";
        public const string AddMediaEarlyTitleMessage = "Too Early to add Photos/Videos";
        public const string AddMediaNotProperTitleMessage = "Info";
        public const string iOSGoogleNavigation = "http://maps.google.com?saddr=My+Location&daddr={0},{1}";
        public const string androidGoogleNavigation = "geo:0,0?q={0},{1}";
        #endregion

        #region Pulse Texts

        public const string UppercaseSubjectText = "SUBJECT";
        public const string SubjectPlaceholderText = "What this pulse is all about...";
        public const string UppercaseLinkedEventText = "LINKED EVENT";
        public const string LinkAnEventText = "Link an Event";
        public const string UppercaseParticipantsText = "PARTICIPANTS";
        public const string AddParticipantsText = "Add Participants";
        public const string NewPulseText = "New Pulse";
        public const string AddText = "Add";
        public const string LeavePulseText = "Leave Pulse";
        public const string ReportSpamText = "Report Spam";
        public const string PulseCreated = "Pulse initiated successfully";
        public const string ClearHistoryTapped = "Clear Chat History";

        #endregion

        #region LinkEvent Page Texts

        public const string LinkEventText = "Link Event";
        public const string DoneText = "Done";
        public const string RemoveText = "Remove";

        #endregion

        #region Notification Titles
        public const string NewPulseNotificationTitle = "New Pulse";
        public const string EventInvitiationNotificationTitle = "Event Invitation";
        public const string EventCancelNotificationTitle = "Event Cancel";
        public const string TagOnCommentNotificationTitle = "Tag On Comment";
        public const string InterestedNotificationTitle = "Interested in Event";
        public const string InviteAcceptNotificationTitle = "Invitation Accepted";
        public const string EventReminderNotificationTitle = "Event Reminder";
        public const string FriendRequestNotificationTitle = "Friend Request";
        public const string FriendRequestAcceptedNotificationTitle = "Friend Request Accepted";
        #endregion
    }
}
