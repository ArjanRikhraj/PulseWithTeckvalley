using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Pulse.Pages.User;
using Xamarin.Forms;

namespace Pulse
{
    public class EventViewModel : BaseViewModel
    {
        #region private variables
        bool isScreenFirstVisible = true;
        bool isScreenSecondVisible = false;
        bool isNoUserFoundVisible;
        bool isListUserVisible;
        bool isNoEventVisible;
        bool isEventListVisible;
        bool isJoinButtonVisible;
        bool isRefreshing;
        bool boostEventConfirmation;
        ObservableCollection<Friend> listFriends;
        ObservableCollection<MyEvents> listMyEvents;
        ObservableCollection<NotificationResponse> lstNotification;
        MainServices mainService;
        DateTime eventFromDate;
        TimeSpan eventFromTime;
        DateTime eventToDate;
        TimeSpan eventToTime;
        string reasonToCancel;
        bool isReportedSpam;
        bool isOwner;
        bool isAdmin;
        bool isGoing;
        bool isAddressListVisible;
        bool isStackOptionVisible;
        bool isStackCommentVisible;
        bool isEditIconOnDetailVisible;
        bool isEndEventVisible;
        bool isUserNotCheckedIn;
        bool isContactNumberAvailable;
        bool isUserCheckedIn;
        bool isBoostedEvent;
        bool isEmailVerified;
        bool isUpdateBoostEvent;
        bool isNotAlreadyBoosted;
        string checkinButtonText;
        string eventPaidButtonText;
        string iconStar;
        string coverPhoto;
        string eventTitle;
        string city;
        string uniqueThumbName;
        string state;
        string country;
        string eventVenue;
        string mobileNumber;
        string coverAmount;
        string bottleAmount;
        string eventBillingAddress;
        string eventBillingCity;
        string eventBillingState;
        string eventBillingZipCode;
        string eventBillingCountry;
        string description;
        string likesCount;
        string eventTime;
        string eventDate;
        string commentText;
        string commentToPost;
        string commenteeName;
        string commentDate;
        string commentId;
        string nameOnCard;
        string cardNumber;
        string eventInviteeStatus;
        double listPlacesHeight;
        string eventLitScore;
        string coverFreeAmountText;
        bool isCoverFreeAmountAvailable;
        bool isUploadCoverImageVisible;
        bool isPartyTextVisible;
        bool isNotInterestedImageVisible;
        bool isInterestedImageVisible;
        bool isNotInterestedIconVisible;
        bool isInterestedIconVisible;
        bool isCheckinDisableButtonVisible;
        bool isLocNoEventVisible = false;
        bool isNoNotificationVisible;
        bool isLocEventListVisible;
        bool isNotificationListVisible;
        string hostedName;
        string hostImage;
        string locationEvent;
        string commenteeImage;
        string attendeeCountLabel;
        string eventReportedData;
        string eventCommentReportedData;
        bool isNotification;
        Color interestedTextColor;
        Color notInterestedTextColor;
        bool isCheckinButtonVisible;
        bool isCommentDeleteIconVisible;
        string cvv;
        ObservableCollection<SearchResultModel> places;
        SearchResultModel searchResultModel;
        ObservableCollection<Friend> listGuests;
        ObservableCollection<MyEvents> listLocBasedEvents;
        ObservableCollection<MyEvents> listLatLongBasedEvents;

        List<Medium> MediaFilesList;
        bool isNoGuestFoundVisible;
        bool isListGuestVisible;
        bool isLikeIconVisible;
        bool isLikedIconVisible;
        Color labelLikeColor;
        bool is_like;
        #endregion

        #region Public properties
        public int pageNoFriend;
        public int pageNoMyEvents;
        public int pageNoNotification;
        public int pageNoLocBasedEvents;
        public int pageNoLatLongBasedEvents;
        public int pageNoGuests;
        public int eventStatus;
        public int TappedEventId;
        public string TappedCommentId;
        public int numberLikes;
        public int pageNoMedia;
        public int pageNoComment;
        public int ExpiryMonth;
        public int totalMediaPages;
        public int totalLiveMediaPages;
        public int totalEventPages;
        public int totalNotificationPages;
        public int totalLocBasedPages;
        public int totalLatlongBasedPages;
        public int totalGuestPages;
        public List<Countries> CountryList;
        public string ExpiryYear;
        public string transactionEmail;
        public string eventDateOnMap;
        public string eventLat;
        public string eventLong;
        public string PaymentEmail;
        public string currenteventLat;
        public string currenteventLong;
        public double eventLattitude;
        public double eventLogitude;
        public bool IsCoverAmount;
        public bool IsBottleAmount;
        public int totalPagesComment;
        public int totalPagesFriends;
        public bool isUserTakenBottleService;
        public string EventCoverAmount;
        public string EventBottleAmount;
        public MyEventType currentActiveEventType;
        public GuestType currentActiveGuestType;
        public List<FriendResponseForUser> UsersList;
        public List<MyEventResponse> EventsList;
        public List<GuestResponse> GuestList;
        public List<NotificationResponse> NotificationList;
        public ICommand UpdateStarEventCommand { get; private set; }
        public ICommand NextButtonClick { get; private set; }
        public ICommand PreviousButtonClick { get; private set; }
        public ICommand CreateEventClick { get; private set; }
        public ICommand GetEventListCommand { get; private set; }
        public ICommand BoostImageTapped { get; private set; }
        public ICommand UpdateEventClick { get; private set; }
        public ICommand CancelEventClick { get; private set; }
        public ICommand LikeEventClick { get; private set; }
        public ICommand PostCommentClick { get; private set; }
        public ICommand PaymentClick { get; private set; }
        public ICommand NoThanksTapped { get; private set; }
        public ObservableCollection<Friend> tempFriendList;
        public List<Attendee> EventAttendeeList;
        public List<EventMedia> EventMediaList;
        public List<string> TimeZoneList;
        public List<EventMedia> MediaList;
        public List<CommentResponse> CommentList;
        public string searchvalue;
        public ObservableCollection<Friend> SelectedFriendsList;
        public ObservableCollection<Friend> UpdatedSelectedFriendsList;
        public ObservableCollection<MediaData> SelectedMediaList;
        public ObservableCollection<Friend> tempGuestList;
        public string TimeZoneType;
        public string EventLocation;
        public string listURL;
        public string filterType;
        public bool IsSearchItemSelected;
        public bool IsUpdateEvent;
        public bool isJoinEvent;
        public bool IsSearchLocItemSelected;
        public bool isCoverFeeChecked;
        public bool isBottleFeeChecked;
        public bool isPublic;
        public bool isMonthSelected;
        public bool isYearSelected;
        public bool isBoostEvent;
        ObservableCollection<GoogleSearchResult> SearchResult;
        public ObservableCollection<MyEvents> tempEventList;
        public ObservableCollection<MyEvents> tempLocBasedEventList;
        public ObservableCollection<MyEvents> tempLatlongBasedEventList;
        public ObservableCollection<MyEvents> tempocBasedEventOnMaps;
        public ObservableCollection<MyEvents> temlatlongBasedEventOnMaps;
        public ObservableCollection<NotificationResponse> tmpNotificationList;
        public string FilterType;
        public string FilterLocType;
        public ICommand LoadMoreEvents { get; private set; }
        public ICommand LoadMoreLatLongEvents { get; private set; }
        public ICommand LoadMoreNotifications { get; private set; }
        public ICommand LoadMoreGuests { get; private set; }
        public double TotalAmount;
        public ICommand LoadMoreLocBasedEvents { get; private set; }
        public ObservableCollection<MyEvents> ListLocBasedEvents
        {
            get { return listLocBasedEvents; }
            set
            {
                listLocBasedEvents = value;
                OnPropertyChanged("ListLocBasedEvents");
            }
        }
        public ObservableCollection<MyEvents> ListLatLongBasedEvents
        {
            get { return listLatLongBasedEvents; }
            set
            {
                listLatLongBasedEvents = value;
                OnPropertyChanged("ListLatLongBasedEvents");
            }
        }
        public ObservableCollection<MyEvents> ListMyEvents
        {
            get { return listMyEvents; }
            set
            {
                listMyEvents = value;
                OnPropertyChanged("ListMyEvents");
            }
        }

        public bool IsScreenFirstVisible
        {
            get { return isScreenFirstVisible; }
            set
            {
                isScreenFirstVisible = value;
                OnPropertyChanged("IsScreenFirstVisible");
            }
        }
        public bool IsScreenSecondVisible
        {
            get { return isScreenSecondVisible; }
            set
            {
                isScreenSecondVisible = value;
                OnPropertyChanged("IsScreenSecondVisible");
            }
        }
        public bool IsNotification
        {
            get { return isNotification; }
            set
            {
                isNotification = value;
                OnPropertyChanged("IsNotification");
            }
        }
        public bool BoostEventConfirmation
        {
            get { return boostEventConfirmation; }
            set
            {
                boostEventConfirmation = value;
                OnPropertyChanged("BoostEventConfirmation");
            }
        }
        public bool IsJoinEvent
        {
            get { return isJoinEvent; }
            set
            {
                isJoinEvent = value;
                OnPropertyChanged("IsJoinEvent");
            }
        }
        public ObservableCollection<NotificationResponse> ListNotification
        {
            get { return lstNotification; }
            set
            {
                lstNotification = value;
                OnPropertyChanged("ListNotification");
            }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public bool IsOwner
        {
            get { return isOwner; }
            set
            {
                isOwner = value;
                OnPropertyChanged("IsOwner");
            }
        }
        public bool IsGoing
        {
            get { return isOwner; }
            set
            {
                isOwner = value;
                OnPropertyChanged("IsGoing");
            }
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        public bool IsUserCheckedIn
        {
            get { return isUserCheckedIn; }
            set
            {
                isUserCheckedIn = value;
                OnPropertyChanged(nameof(IsUserCheckedIn));
            }
        }
        public bool IsBoostEvent
        {
            get { return isBoostedEvent; }
            set
            {
                isBoostedEvent = value;
                OnPropertyChanged(nameof(IsBoostEvent));
            }
        }
        public bool IsEmailVerified
        {
            get { return isEmailVerified; }
            set
            {
                isEmailVerified = value;
                OnPropertyChanged(nameof(IsBoostEvent));
            }
        }
        public bool IsUpdateBoostEvent
        {
            get { return isUpdateBoostEvent; }
            set
            {
                isUpdateBoostEvent = value;
                OnPropertyChanged(nameof(IsUpdateBoostEvent));
            }
        }
        public bool IsNotAlreadyBoosted
        {
            get { return isNotAlreadyBoosted; }
            set
            {
                isNotAlreadyBoosted = value;
                OnPropertyChanged(nameof(IsNotAlreadyBoosted));
            }
        }
        public bool IsUserNotCheckedIn
        {
            get { return isUserNotCheckedIn; }
            set
            {
                isUserNotCheckedIn = value;
                OnPropertyChanged(nameof(IsUserNotCheckedIn));
            }
        }
        public bool IsContactNumberAvailable
        {
            get { return isContactNumberAvailable; }
            set
            {
                isContactNumberAvailable = value;
                OnPropertyChanged(nameof(IsContactNumberAvailable));
            }
        }

        public bool IsLocEventListVisible
        {
            get { return isLocEventListVisible; }
            set
            {
                isLocEventListVisible = value;
                OnPropertyChanged("IsLocEventListVisible");
            }
        }

        public bool IsNotificationListVisible
        {
            get { return isNotificationListVisible; }
            set
            {
                isNotificationListVisible = value;
                OnPropertyChanged("IsNotificationListVisible");
            }
        }

        public bool IsEditIconOnDetailVisible
        {
            get { return isEditIconOnDetailVisible; }
            set
            {
                isEditIconOnDetailVisible = value;
                OnPropertyChanged("IsEditIconOnDetailVisible");
            }
        }
        public bool IsEndEventVisible
        {
            get { return isEndEventVisible; }
            set
            {
                isEndEventVisible = value;
                OnPropertyChanged("IsEndEventVisible");
            }
        }
        public bool IsLocNoEventVisible
        {
            get { return isLocNoEventVisible; }
            set
            {
                isLocNoEventVisible = value;
                OnPropertyChanged("IsLocNoEventVisible");
            }
        }
        public bool IsNoNotificationVisible
        {
            get { return isNoNotificationVisible; }
            set
            {
                isNoNotificationVisible = value;
                OnPropertyChanged("IsNoNotificationVisible");
            }
        }
        public string CVV
        {
            get
            {
                return cvv;
            }
            set
            {
                cvv = value;
                OnPropertyChanged("CVV");
            }
        }
        public bool IsNoGuestFoundVisible
        {
            get { return isNoGuestFoundVisible; }
            set
            {
                isNoGuestFoundVisible = value;
                OnPropertyChanged("IsNoGuestFoundVisible");
            }
        }
        public bool IsStackCommentVisible
        {
            get { return isStackCommentVisible; }
            set
            {
                isStackCommentVisible = value;
                OnPropertyChanged("IsStackCommentVisible");
            }
        }

        public bool IsCommentDeleteIconVisible
        {
            get { return isCommentDeleteIconVisible; }
            set
            {
                isCommentDeleteIconVisible = value;
                OnPropertyChanged("IsCommentDeleteIconVisible");
            }
        }
        public string CheckinButtonText
        {
            get { return checkinButtonText; }
            set
            {
                checkinButtonText = value;
                OnPropertyChanged("CheckinButtonText");
            }
        }
        public string EventPaidButtonText
        {
            get { return eventPaidButtonText; }
            set
            {
                eventPaidButtonText = value;
                OnPropertyChanged("EventPaidButtonText");
            }
        }
        public string AttendeeCountLabel
        {
            get { return attendeeCountLabel; }
            set
            {
                attendeeCountLabel = value;
                OnPropertyChanged("AttendeeCountLabel");
            }
        }
        public string EventBillingAddress
        {
            get { return eventBillingAddress; }
            set
            {
                eventBillingAddress = value;
                OnPropertyChanged("EventBillingAddress");
            }
        }
        public string EventBillingCity
        {
            get { return eventBillingCity; }
            set
            {
                eventBillingCity = value;
                OnPropertyChanged("EventBillingCity");
            }
        }
        public string EventBillingState
        {
            get { return eventBillingState; }
            set
            {
                eventBillingState = value;
                OnPropertyChanged("EventBillingState");
            }
        }
        public string EventBillingZipCode
        {
            get { return eventBillingZipCode; }
            set
            {
                eventBillingZipCode = value;
                OnPropertyChanged("EventBillingZipCode");
            }
        }
        public string EventBillingCountry
        {
            get { return eventBillingCountry; }
            set
            {
                eventBillingCountry = value;
                OnPropertyChanged("EventBillingCountry");
            }
        }

        public string CommentId
        {
            get { return commentId; }
            set
            {
                commentId = value;
                OnPropertyChanged("CommentId");
            }
        }
        public string CommenteeImage
        {
            get { return commenteeImage; }
            set
            {
                commenteeImage = value;
                OnPropertyChanged("CommenteeImage");
            }
        }
        public string CommentToPost
        {
            get { return commentToPost; }
            set
            {
                commentToPost = value;
                OnPropertyChanged("CommentToPost");
            }
        }
        public string EvenetReportedData
        {
            get { return eventReportedData; }
            set
            {
                eventReportedData = value;
                OnPropertyChanged("EvenetReportedData");
            }
        }
        public string EvenetCommentReportedData
        {
            get { return eventCommentReportedData; }
            set
            {
                eventCommentReportedData = value;
                OnPropertyChanged("EvenetCommentReportedData");
            }
        }
        public string CommenteeName
        {
            get { return commenteeName; }
            set
            {
                commenteeName = value;
                OnPropertyChanged("CommenteeName");
            }
        }
        public string CommentDate
        {
            get { return commentDate; }
            set
            {
                commentDate = value;
                OnPropertyChanged("CommentDate");
            }
        }

        public bool IsListGuestVisible
        {
            get { return isListGuestVisible; }
            set
            {

                isListGuestVisible = value;
                OnPropertyChanged("IsListGuestVisible");

            }
        }

        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged("CommentText");
            }
        }
        public string HostedName
        {
            get { return hostedName; }
            set
            {
                hostedName = value;
                OnPropertyChanged("HostedName");
            }
        }
        public string LocationEvent
        {
            get { return locationEvent; }
            set
            {
                locationEvent = value;
                OnPropertyChanged("LocationEvent");
            }
        }
        public string LikesCount
        {
            get { return likesCount; }
            set
            {
                likesCount = value;
                OnPropertyChanged("LikesCount");
            }
        }
        public ObservableCollection<Friend> ListGuests
        {
            get { return listGuests; }
            set
            {
                listGuests = value;
                OnPropertyChanged("ListGuests");
            }
        }
        public string EventLitScore
        {
            get { return eventLitScore; }
            set
            {
                eventLitScore = value;
                OnPropertyChanged("EventLitScore");
            }
        }
        public string EventDate
        {
            get { return eventDate; }
            set
            {
                eventDate = value;
                OnPropertyChanged("EventDate");
            }
        }
        public string EventTime
        {
            get { return eventTime; }
            set
            {
                eventTime = value;
                OnPropertyChanged("EventTime");
            }
        }
        public string HostImage
        {
            get { return hostImage; }
            set
            {
                hostImage = value;
                OnPropertyChanged("HostImage");
            }
        }
        public bool IsLikeIconVisible
        {
            get
            {
                return isLikeIconVisible;
            }
            set
            {
                isLikeIconVisible = value;
                OnPropertyChanged("IsLikeIconVisible");
            }
        }
        public string NameOnCard
        {
            get
            {
                return nameOnCard;
            }
            set
            {
                nameOnCard = value;
                OnPropertyChanged("NameOnCard");
            }
        }
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
                OnPropertyChanged("CardNumber");
            }
        }
        public bool IsLikedIconVisible
        {
            get
            {
                return isLikedIconVisible;
            }
            set
            {
                isLikedIconVisible = value;
                OnPropertyChanged("IsLikedIconVisible");
            }
        }
        public bool IsCheckinDisableButtonVisible
        {
            get
            {
                return isCheckinDisableButtonVisible;
            }
            set
            {
                isCheckinDisableButtonVisible = value;
                OnPropertyChanged("IsCheckinDisableButtonVisible");
            }
        }
        public Color LabelLikeColor
        {
            get { return labelLikeColor; }
            set
            {
                labelLikeColor = value;
                OnPropertyChanged("LabelLikeColor");
            }
        }

        public Color InterestedTextColor
        {
            get { return interestedTextColor; }
            set
            {
                interestedTextColor = value;
                OnPropertyChanged("InterestedTextColor");
            }
        }
        public Color NotInterestedTextColor
        {
            get { return notInterestedTextColor; }
            set
            {
                notInterestedTextColor = value;
                OnPropertyChanged("NotInterestedTextColor");
            }
        }
        public bool IsInterestedImageVisible
        {
            get { return isInterestedImageVisible; }
            set
            {
                isInterestedImageVisible = value;
                OnPropertyChanged("IsInterestedImageVisible");
            }
        }
        public bool IsNotInterestedImageVisible
        {
            get { return isNotInterestedImageVisible; }
            set
            {
                isNotInterestedImageVisible = value;
                OnPropertyChanged("IsNotInterestedImageVisible");
            }
        }
        public bool IsInterestedIconVisible
        {
            get { return isInterestedIconVisible; }
            set
            {
                isInterestedIconVisible = value;
                OnPropertyChanged("IsInterestedIconVisible");
            }
        }



        public bool IsNotInterestedIconVisible
        {
            get { return isNotInterestedIconVisible; }
            set
            {
                isNotInterestedIconVisible = value;
                OnPropertyChanged("IsNotInterestedIconVisible");
            }
        }

        public string CoverFreeAmountText
        {
            get { return coverFreeAmountText; }
            set
            {
                coverFreeAmountText = value;
                OnPropertyChanged("CoverFreeAmountText");
            }
        }

        public bool IsJoinButtonVisible
        {
            get { return isJoinButtonVisible; }
            set
            {
                isJoinButtonVisible = value;
                OnPropertyChanged("IsJoinButtonVisible");
            }
        }
        public bool IsCheckinButtonVisible
        {
            get { return isCheckinButtonVisible; }
            set
            {
                isCheckinButtonVisible = value;
                OnPropertyChanged("IsCheckinButtonVisible");
            }
        }
        public bool IsStackOptionVisible
        {
            get { return isStackOptionVisible; }
            set
            {
                isStackOptionVisible = value;
                OnPropertyChanged("IsStackOptionVisible");
            }
        }
        public bool IsUploadCoverImageVisible
        {
            get { return isUploadCoverImageVisible; }
            set
            {
                isUploadCoverImageVisible = value;
                OnPropertyChanged("IsUploadCoverImageVisible");
            }
        }
        public bool IsPartyTextVisible
        {
            get { return isPartyTextVisible; }
            set
            {
                isPartyTextVisible = value;
                OnPropertyChanged("IsPartyTextVisible");
            }
        }

        public bool IsCoverFreeAmountAvailable
        {
            get { return isCoverFreeAmountAvailable; }
            set
            {
                isCoverFreeAmountAvailable = value;
                OnPropertyChanged("IsCoverFreeAmountAvailable");
            }
        }
        public double ListPlacesHeight
        {
            get
            {
                return listPlacesHeight;
            }
            set
            {
                listPlacesHeight = value;
                OnPropertyChanged("ListPlacesHeight");
            }
        }
        public ObservableCollection<SearchResultModel> Places
        {
            get
            {
                return places;
            }
            set
            {
                places = value;
                OnPropertyChanged("Places");
            }
        }
        public ObservableCollection<Friend> ListFriends
        {
            get { return listFriends; }
            set
            {
                listFriends = value;
                OnPropertyChanged("ListFriends");
            }
        }


        public bool IsEventListVisible
        {
            get { return isEventListVisible; }
            set
            {
                isEventListVisible = value;
                OnPropertyChanged("IsEventListVisible");
            }
        }
        public bool IsNoEventVisible
        {
            get { return isNoEventVisible; }
            set
            {
                isNoEventVisible = value;
                OnPropertyChanged("IsNoEventVisible");
            }
        }
        public bool IsNoUserFoundVisible
        {
            get { return isNoUserFoundVisible; }
            set
            {
                isNoUserFoundVisible = value;
                OnPropertyChanged("IsNoUserFoundVisible");
            }
        }
        public string ReasonToCancel
        {
            get { return reasonToCancel; }
            set
            {
                reasonToCancel = value;
                OnPropertyChanged("ReasonToCancel");
            }
        }
        public bool IsListUserVisible
        {
            get { return isListUserVisible; }
            set
            {
                isListUserVisible = value;
                OnPropertyChanged("IsListUserVisible");
            }
        }
        public bool IsAddressListVisible
        {
            get { return isAddressListVisible; }
            set
            {
                isAddressListVisible = value;
                OnPropertyChanged("IsAddressListVisible");
            }

        }
        public string EventTitle
        {
            get { return eventTitle; }
            set
            {
                eventTitle = value;
                OnPropertyChanged("EventTitle");

            }
        }
        public string IconStar
        {
            get { return iconStar; }
            set
            {
                iconStar = value;
                OnPropertyChanged("IconStar");

            }
        }
        public string CoverPhoto
        {
            get { return coverPhoto; }
            set
            {
                coverPhoto = value;
                OnPropertyChanged("CoverPhoto");

            }
        }
        public string EventInviteeStatus
        {
            get { return eventInviteeStatus; }
            set
            {
                eventInviteeStatus = value;
                OnPropertyChanged("EventInviteeStatus");

            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");

            }
        }

        public string EventVenue
        {
            get { return eventVenue; }
            set
            {
                eventVenue = value;
                OnPropertyChanged("EventVenue");

            }
        }
        public string MobileNumber
        {
            get { return mobileNumber; }
            set
            {
                mobileNumber = value;
                OnPropertyChanged("MobileNumber");

            }
        }
        public string CoverAmount
        {
            get { return coverAmount; }
            set
            {
                coverAmount = value;
                OnPropertyChanged("CoverAmount");

            }
        }
        public string BottleAmount
        {
            get { return bottleAmount; }
            set
            {
                bottleAmount = value;
                OnPropertyChanged("BottleAmount");

            }
        }
        public SearchResultModel ResultModel

        {
            get
            {
                return searchResultModel;
            }
            set
            {
                searchResultModel = value;
                OnPropertyChanged("ResultModel");
            }
        }
        public DateTime EventFromDate
        {
            get { return eventFromDate; }
            set
            {
                eventFromDate = value;
                OnPropertyChanged("EventFromDate");
            }
        }
        public TimeSpan EventFromTime
        {
            get { return eventFromTime; }
            set
            {
                eventFromTime = value;
                OnPropertyChanged("EventFromTime");
            }
        }

        public TimeSpan EventToTime
        {
            get { return eventToTime; }
            set
            {
                eventToTime = value;
                OnPropertyChanged("EventToTime");
            }
        }
        public bool IsReportedSpam
        {
            get { return isReportedSpam; }
            set
            {
                isReportedSpam = value;
                OnPropertyChanged("IsReportedSpam");
            }
        }
        public DateTime EventToDate
        {
            get { return eventToDate; }
            set
            {
                eventToDate = value;
                OnPropertyChanged("EventToDate");
            }
        }
        //new Value
        private ObservableCollection<MyEvents> allUpcomingEvents { get; set; }
        public ObservableCollection<MyEvents> AllUpcomingEvents
        {
            get { return allUpcomingEvents; }
            set
            {
                allUpcomingEvents = value;
                OnPropertyChanged("AllUpcomingEvents");
            }
        }
        private DateTime fromDate;
        public DateTime FromDate
        {
            get
            {
                
                return fromDate;
            }
            set
            {
                    fromDate = value;
                    OnPropertyChanged("FromDate");
            }
        }
        private DateTime toDate;
        public DateTime ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                    toDate = value;
                    OnPropertyChanged("ToDate");
            }
        }
        #endregion

        //New Variables And Commands
        private ICommand checkInCommand { get; set; }
        public ICommand CheckInCommand
        {
            get
            {
                return checkInCommand ?? (checkInCommand = new Command<MyEvents>((currentObject) => FetchEventDetail(currentObject.EventId.ToString(),false)));
            }
        }
        private bool addressListLoading { get; set; } = false;

        public bool AddressListLoading
        {
            get { return addressListLoading; }
            set
            {
                addressListLoading = value;
                OnPropertyChanged("AddressListLoading");
            }

        }
        DateTime selectedDate = DateTime.Now.Date;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
                GetEventsByDate(selectedDate);
            }
        }

        private ICommand starEventCommand { get; set; }
        public ICommand StarEventCommand
        {
            get
            {
                return starEventCommand ?? (starEventCommand = new Command<object>((currentObject) => CreateStarredEvent(currentObject)));
            }
        }
        private EventGallery mediaSelectedItem;
        public EventGallery MediaSelectedItem
        {
            get
            {
                return mediaSelectedItem;
            }
            set
            {
                mediaSelectedItem = value;
                OnPropertyChanged("MediaSelectedItem");
                if(mediaSelectedItem!=null)
                ShowMedia(MediaSelectedItem);
            }
        }

        private string cover_Photo { get; set; }
        public string Cover_Photo
        {
            get { return cover_Photo; }
            set
            {
                cover_Photo = value;
                OnPropertyChanged("Cover_Photo");
            }
        }
        #region Constructor
        public EventViewModel()
        {
            mainService = new MainServices();
            UpdateUserLocation();
            tempFriendList = new ObservableCollection<Friend>();
            tempGuestList = new ObservableCollection<Friend>();
            SelectedFriendsList = new ObservableCollection<Friend>();
            UpdatedSelectedFriendsList = new ObservableCollection<Friend>();
            SelectedMediaList = new ObservableCollection<MediaData>();
            Places = new ObservableCollection<SearchResultModel>();
            tempEventList = new ObservableCollection<MyEvents>();
            tmpNotificationList = new ObservableCollection<NotificationResponse>();
            ListNotification = new ObservableCollection<NotificationResponse>();
            TimeZoneList = new List<string>();
            MediaFilesList = new List<Medium>();
            // GetEventListCommand = new Command();
            NextButtonClick = new Command(NextScreen);
            PreviousButtonClick = new Command(PreviousScreen);
            CreateEventClick = new Command(CreateEvent);
            UpdateEventClick = new Command(UpdateEvent);
            BoostImageTapped = new Command(BoostImageClick);
            //CancelEventClick = new Command(CancelEvent);
            LikeEventClick = new Command(LikeEvent);
            LoadMoreEvents = new Command(GetMyEventsList);
            LoadMoreLatLongEvents = new Command(GetMapLatLongEvents);
            LoadMoreNotifications = new Command(GetNotificationList);
            LoadMoreGuests = new Command(GetGuests);
            NoThanksTapped = new Command(NoThanksClick);
            LoadMoreLocBasedEvents = new Command(GetLocBasedEvents);
            tempLocBasedEventList = new ObservableCollection<MyEvents>();
            tempLatlongBasedEventList = new ObservableCollection<MyEvents>();
            //PostCommentClick = new Command(PostComment);
            PaymentClick = new Command(MakePayment);
            EventAttendeeList = new List<Attendee>();
            EventMediaList = new List<EventMedia>();
            NotificationList = new List<NotificationResponse>();
        }
        #endregion
        #region Methods
        public async void GetUserPaymentCardDetails()
        {
            try
            {
                if(SessionManager.AccessToken!=null)
                {
                    var response = await mainService.Get<ResultWrapperSingle<CreditCardDetailsResponse>>(Constant.GetUserCreditCardDetailsUrl);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        //CardNumber = response.response.card_no="1233335436636";
                        ////ExpiryYear = response.response.expiry_year=2025;
                        //ExpiryMonth = response.response.expiry_month=11;
                        //EventBillingAddress = response.response.address="AIG Park avenue, gaur city";
                        //EventBillingCity = response.response.city="Noida";
                        //EventBillingState = response.response.state="Uttar Pradesh";
                        //EventBillingZipCode = response.response.postal_code="201009";
                        //NameOnCard = response.response.name="Ajay";
                        //EventBillingCountry = response.response.country="India";
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void CreateStarredEvent(object Sender)
        {
            try
            {
                var currentObject = (MyEvents)Sender;
                StarEventRequest starEventRequest = new StarEventRequest();
                starEventRequest.user_id= SessionManager.UserId;
                starEventRequest.event_id = currentObject != null ? currentObject.EventId : TappedEventId;
                if (starEventRequest == null)
                    return;
                var response = await mainService.Post<ResultWrapperSingle<StarEventResponse>>(Constant.StarEventUrl, starEventRequest);
                if (response != null && response.status == Constant.Status200 && response.response != null)
                {
                    GetAllUpComingEvents();
                    if (currentObject == null)
                        IconStar = response.response.is_star? "iconStarred.png" : "iconStar.png";
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void ShowMedia(EventGallery selectedItem)
        {
            try
            {
               await Navigation.PushModalAsync(new ShowMedia(selectedItem));
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        //private async void PinMedia(object sender)
        //{
        //    try
        //    {
        //        var obj= (EventGallery)sender;
        //        if(obj!=null)
        //        {
        //            PinMediaRequest request = new PinMediaRequest();
        //            request.story_id = obj.MediaId;
        //            request.user_id = SessionManager.UserId;
        //            if (request == null)
        //                return;
        //            var response = await mainService.Post<ResultWrapperSingle<StarEventResponse>>(Constant.PinStoryUrl, request);
        //            if (response != null && response.status == Constant.Status200 && response.response != null)
        //            {
        //                if (currentObject == null)
        //                    IconStar = response.response.is_star ? "iconStarred.png" : "iconStar.png";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
        //    }
        //}
        private void GetEventsByDate(DateTime selectedDate)
        {
            try
            {
                if (AllUpcomingEvents == null)
                    return;
                var list = tempEventList.Where(x => x.StartDate.Date == selectedDate.Date);
                AllUpcomingEvents = new ObservableCollection<MyEvents>(list);
               // AllUpcomingEvents = tempEventList;
                if(AllUpcomingEvents.Count==0)
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "No event found for selected date", "Ok");
                    GetAllUpComingEvents();
                    SelectedDate = DateTime.Now.Date;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async void GetEventsByFilter(string item)
        {
            try
            {
                if (ListLocBasedEvents == null)
                    return;
                if(item=="SOON")
                {
                    var list = ListLocBasedEvents.Where(x => x.StartDateTime <= DateTime.Now.AddMinutes(180) && x.StartDateTime>DateTime.Now);
                    tempLocBasedEventList = new ObservableCollection<MyEvents>(list);
                    if (tempLocBasedEventList.Count == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "No event starting soon", "Ok");
                        SelectedDate = DateTime.Now.Date;
                       await GetMapLocBasedEvents();
                    }
                }
                else if(item== "LIVE")
                {
                    var list = ListLocBasedEvents.Where(x => x.EventStatus=="Hosting");
                    tempLocBasedEventList = new ObservableCollection<MyEvents>(list);
                    if (tempLocBasedEventList.Count == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "No live event found", "Ok");
                        SelectedDate = DateTime.Now.Date;
                        await GetMapLocBasedEvents();
                    }
                }
                else if (item == "ALL")
                {
                    var list = ListLocBasedEvents.Where(x => x.StartDateTime <= DateTime.Now.AddMinutes(180) || x.StartDateTime <= DateTime.Now);
                    tempLocBasedEventList = new ObservableCollection<MyEvents>(list);
                    if (tempLocBasedEventList.Count == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "No event is happening", "Ok");
                        SelectedDate = DateTime.Now.Date;
                        await GetMapLocBasedEvents();
                    }
                }
                else
                    GetAllUpComingEvents();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async Task<bool> MarkInappropriate(string comentText, string mediaId)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    InapprorpriateMedia media = new InapprorpriateMedia();
                    media.comment = comentText;
                    media.in_appropriate = true;
                    if (SessionManager.AccessToken != null)
                    {
                        var response = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.MarkInappropriateUrl + mediaId + "/", media);
                        if (response != null && response.status == Constant.Status200)
                        {
                            return true;
                        }

                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> GetSearchedFriends(string searchKeyword, string page)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {

                    if (SessionManager.AccessToken != null && (pageNoFriend <= totalPagesFriends || pageNoFriend == 1))
                    {
                        UsersList = new List<FriendResponseForUser>();
                        string url;
                        if (page.Equals("AddEvent"))
                        {
                            url = Constant.SearchFriendUrl + pageNoFriend + Constant.SearchString + searchKeyword;
                        }
                        else
                        {
                            url = Constant.TagFriendsUrl + TappedEventId + "/?page=" + pageNoFriend;
                        }
                        var response = await mainService.Get<ResultWrapper<FriendResponseForUser>>(url);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                UsersList.Add(item);
                            }
                            totalPagesFriends = GetPageCount(response.response[response.response.Count - 1].total_users);
                            return true;
                        }

                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        void SetLikeCount()
        {
            if (numberLikes > 0)
            {
                if (is_like)
                {
                    IsLikeIconVisible = false;
                    IsLikedIconVisible = true;
                    LabelLikeColor = Color.FromHex(Constant.PinkButtonColor);

                }
                else
                {
                    IsLikeIconVisible = true;
                    IsLikedIconVisible = false;
                    LabelLikeColor = Color.FromHex(Constant.FilterLightGreyColor);

                }
                //LabelLikeColor = Color.FromHex(Constant.FilterLightGreyColor);
                LikesCount = numberLikes > 0 && numberLikes < 2 ? numberLikes.ToString() + Constant.LikeText : numberLikes.ToString() + Constant.LikesText;
            }
            else
            {
                //  LabelLikeColor = Color.Transparent;
                LabelLikeColor = Color.FromHex(Constant.FilterLightGreyColor);
                LikesCount = numberLikes.ToString() + Constant.LikeText;
                IsLikeIconVisible = true;
                IsLikedIconVisible = false;
            }
        }
        public async Task GetTimeZones()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (SessionManager.AccessToken != null)
                    {
                        TimeZoneList = new List<string>();
                        var response = await mainService.Get<ResultWrapperSingle<TimeZones>>(Constant.TimeZoneUrl);
                        if (response != null && response.status == Constant.Status200 && response.response != null)
                        {
                            foreach (var item in response.response.zone_names)
                            {
                                TimeZoneList.Add(item);
                            }
                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                        }
                    }
                    else
                    {
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        public async Task GetCountries()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (SessionManager.AccessToken != null)
                    {
                        CountryList = new List<Countries>();
                        var response = await mainService.Get<ResultWrapper<Countries>>(Constant.GetCountriesurl);
                        if (response != null && response.status == Constant.Status200 && response.response != null)
                        {
                            foreach (var item in response.response)
                            {
                                CountryList.Add(item);
                            }
                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                        }
                    }
                    else
                    {
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception e)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        bool Validate()
        {
            if (string.IsNullOrEmpty(EventTitle))
            {
                App.Instance.Alert(Constant.EventTitleRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!IsSearchItemSelected)
            {
                App.Instance.Alert(Constant.EventVenueRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            //else if (string.IsNullOrEmpty(EventVenue))
            //{
            //    App.Instance.Alert(Constant.EventAddressRequired, Constant.AlertTitle, Constant.Ok);
            //    return false;
            //}
            else if (!string.IsNullOrEmpty(MobileNumber) && MobileNumber.Length < 9)
            {
                App.Instance.Alert(Constant.ValidMobileNumberRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!string.IsNullOrEmpty(MobileNumber) && !Regex.IsMatch(MobileNumber, Constant.InvalidNumericRegex))
            {
                App.Instance.Alert(Constant.ValidMobileNumberRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!DateTimeValidate())
            {
                return false;
            }
            else if (isCoverFeeChecked && string.IsNullOrEmpty(CoverAmount))
            {
                App.Instance.Alert(Constant.CoverAmountRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (isCoverFeeChecked && !string.IsNullOrEmpty(CoverAmount) && !CheckDecimal(CoverAmount))
            {
                App.Instance.Alert(Constant.CoverAmountValid, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            //else if (isCoverFeeChecked && !string.IsNullOrEmpty(CoverAmount))
            //{
            //  App.Instance.Alert(Constant.CoverAmountRequired, Constant.AlertTitle, Constant.Ok);
            //  return false;
            //}

            else if (isBottleFeeChecked && string.IsNullOrEmpty(BottleAmount))
            {
                App.Instance.Alert(Constant.BottleAmountRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (isBottleFeeChecked && !string.IsNullOrEmpty(BottleAmount) && !CheckDecimal(BottleAmount))
            {
                App.Instance.Alert(Constant.BottleAmountValid, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            //else if (isCoverFeeChecked && !string.IsNullOrEmpty(CoverAmount) && Convert.ToDouble(BottleAmount) <= 0.0)
            //{
            //  App.Instance.Alert(Constant.BottleAmountRequired, Constant.AlertTitle, Constant.Ok);
            //  return false;
            //}
            else
                return true;

        }
        bool ValidateFirstScreen()
        {
            if (string.IsNullOrEmpty(EventTitle))
            {
                App.Instance.Alert(Constant.EventTitleRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!IsSearchItemSelected)
            {
                App.Instance.Alert(Constant.EventVenueRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!DateTimeValidate())
            {
                return false;
            }
            else
                return true;

        }
        bool DateTimeValidate()
        {
            if (EventFromDate < DateTime.Now.Date || EventToDate < DateTime.Now.Date)
            {
                App.Instance.Alert(Constant.PastDateCannotSelectMessage, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else
            {
                if (EventFromDate.Date > EventToDate.Date)
                {
                    App.Instance.Alert(Constant.EndDateGreaterThenStartDateMessage, Constant.AlertTitle, Constant.Ok);
                    return false;
                }
                else if (EventFromDate.Date == DateTime.Now.Date && EventFromTime < DateTime.Now.TimeOfDay)
                {
                    App.Instance.Alert(Constant.PastTimeCannotSelectMessage, Constant.AlertTitle, Constant.Ok);
                    return false;
                }
                else if (EventToDate.Date == DateTime.Now.Date && EventToTime < DateTime.Now.TimeOfDay)
                {
                    App.Instance.Alert(Constant.PastTimeCannotSelectMessage, Constant.MessgeTitle, Constant.Ok);
                    return false;
                }
                else if (EventFromDate.Date == EventToDate.Date && EventFromTime >= EventToTime)
                {
                    App.Instance.Alert(Constant.EndTimeGreaterThenStartTimeMessage, Constant.AlertTitle, Constant.Ok);
                    return false;
                }
                else
                    return true;
            }
        }
        bool CheckDecimal(string data)
        {
            bool isDecimal = false;
            bool isValidDecimal = false;
            bool isValidNumeric = false;
            foreach (char c in data)
            {
                if (char.IsDigit(c))
                {
                    isValidNumeric = true;
                }
                else if (c.ToString() == ".")
                {
                    if (!isDecimal)
                    {
                        isDecimal = true;
                        isValidDecimal = data.IndexOf(c) > 0 && data.IndexOf(c) != data.Length - 1;
                    }
                    else
                    {
                        isValidDecimal = false;
                    }
                }
                else
                {
                    isValidDecimal = false;
                    isValidNumeric = false;
                }
            }
            return isDecimal ? isValidDecimal && isValidNumeric : isValidNumeric;
        }
        public void Search(TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                AddressListLoading = true;
                SearchLocation(e.NewTextValue);
            }
            else
            {
                SearchResult = new ObservableCollection<GoogleSearchResult>(new List<GoogleSearchResult>());
            }
        }
        async void BoostImageClick()
        {
            IsBoostEvent = true;
            TotalAmount = 5.0;
            IsEmailVerified = false;
            //if(string.IsNullOrEmpty(SessionManager.Email))
            //{
            //    SessionManager.Email = string.Empty;
            //}
            //AddEventPage boost event without payment here for test purpose
            await UpdateBoostedEvent();
           // await Navigation.PushModalAsync(new PaymentDetailPage(transactionEmail));

        }
        async void NoThanksClick()
        {
            ClearFields();
            IsBoostEvent = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await App.Instance.Alert(Constant.AlertTitle, Constant.NetworkDisabled, Constant.Ok);
            }
        }
        public async void SearchLocation(string value)
        {
            IsAddressListVisible = false;
            if (value.Length >= Constant.SearchCharacterLimit)
            {
                try
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);

                    }
                    else
                    {
                        IsAddressListVisible = true;
                        var destinationResult = await new PlacesManager().GetGooglePlacesDataAsync(value);
                        var t = from p in destinationResult.predictions select p.terms[1];
                        var tempList = destinationResult.predictions.Select(g => new SearchResultModel
                        {
                            Name = g.description,
                            //Country = g.terms.LastOrDefault().value,
                            //Vicinity = g.description.Substring(g.description.IndexOf(",", StringComparison.Ordinal) + 1),
                            place_id = g.place_id
                        }).ToList();

                        if (tempList.Count != 0)
                        {
                            ListPlacesHeight = tempList.Count * 42;
                            Places = new ObservableCollection<SearchResultModel>(tempList);
                        }
                        else
                        {
                            tempList.Add(new SearchResultModel
                            {
                                Name = Constant.NoResultFound
                            });
                            ListPlacesHeight = 42;
                            Places = new ObservableCollection<SearchResultModel>(tempList);
                        }
                    }
                    AddressListLoading = false;
                }
                catch (Exception)
                {
                    await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                }
                finally
                {
                    AddressListLoading = false;
                }
            }
        }
        public async Task GetlatLong(string place_id)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);

                }
                else
                {
                    var response = await new PlacesManager().GetPlaceDetail(place_id);
                    if (response != null)
                    {
                        eventLattitude = response.result.geometry.location.lat;
                        eventLogitude = response.result.geometry.location.lng;
                        eventLat = Convert.ToString(response.result.geometry.location.lat);
                        eventLong = Convert.ToString(response.result.geometry.location.lng);
                    }
                }

            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        async void NextScreen()
        {
            try
            {
                if (TapCount < 1)
                {
                    if (ValidateFirstScreen())
                    {
                        if (!isScreenSecondVisible && isScreenFirstVisible)
                        {
                            IsScreenFirstVisible = false;
                            IsScreenSecondVisible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }
        async void PreviousScreen()
        {
            try
            {
                if (TapCount < 1)
                {
                    if (isScreenSecondVisible && !isScreenFirstVisible)
                    {
                        IsScreenFirstVisible = true;
                        IsScreenSecondVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }
        async void CreateEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (Validate())
                            {
                                if (!isPublic && SelectedFriendsList.Count <= 0)
                                {
                                    await App.Instance.Alert(Constant.GuestsRequired, Constant.AlertTitle, Constant.Ok);
                                    TapCount = 0;
                                }
                                else
                                {
                                    IsLoading = true;
                                    await UploadMedia();
                                    await UploadThumbnail();
                                    UpdatedSelectedFriendsList.Clear();
                                    var response = await mainService.Post<ResultWrapperSingle<EventResponse>>(Constant.CreateEventUrl, GetCreateEventData());
                                    if (response != null && response.status == Constant.Status200 && response.response != null)
                                    {
                                        if (!string.IsNullOrEmpty(response.response.transaction_email))
                                        {
                                            transactionEmail = response.response.transaction_email;
                                        }
                                        else if (!string.IsNullOrEmpty(SessionManager.Email))
                                        {
                                            transactionEmail = SessionManager.Email;
                                        }
                                        else
                                        {
                                            transactionEmail = string.Empty;
                                        }
                                        await Navigation.PushModalAsync(new BoostEventPopUpPage(), false);
                                        TappedEventId = response.response.id;
                                        IsLoading = false;
                                        TapCount = 0;
                                    }
                                    else if (response != null && response.status == Constant.Status401)
                                    {
                                        SignOut();
                                        IsLoading = false;
                                    }
                                    else
                                    {
                                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                        IsLoading = false;
                                        TapCount = 0;
                                    }
                                }
                            }
                            else
                            {
                                TapCount = 0;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsLoading = false;
                            TapCount = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }
        public async Task CancelEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (!string.IsNullOrEmpty(ReasonToCancel))
                            {
                                IsLoading = true;
                                CancelEvent cancelEv = new CancelEvent();
                                cancelEv.comment = ReasonToCancel;
                                var response = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.CancelEventUrl + Convert.ToString(TappedEventId) + "/", cancelEv);
                                if (response != null && response.status == Constant.Status200 && response.response != null)
                                {
                                    ClearFields();
                                    //await App.Instance.Alert(Constant.EventCancelled, Constant.AlertTitle, Constant.Ok);
                                    ShowToast(Constant.AlertTitle, Constant.EventCancelled);
                                    Application.Current.MainPage = new NavigationPage(new MainPage());
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                                else if (response != null && response.status == Constant.Status401)
                                {
                                    SignOut();
                                    IsLoading = false;
                                }
                                else
                                {
                                    await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.MentionReasonToCancel, Constant.AlertTitle, Constant.Ok);
                                IsLoading = false;
                                TapCount = 0;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsLoading = false;
                            TapCount = 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }
        async void UpdateEvent()
        {
            IsUpdateEvent = true;
            if (IsNotAlreadyBoosted && IsUpdateBoostEvent)
            {
                TotalAmount = 5.0;
                await Navigation.PushModalAsync(new PaymentDetailPage(transactionEmail));
            }
            else
            {
                UpdateExistingEvent();
            }
        }
        async void UpdateExistingEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (Validate())
                            {
                                IsLoading = true;
                                var response = await mainService.Put<ResultWrapperSingle<EventResponse>>(Constant.UpdateEventUrl + Convert.ToString(TappedEventId) + "/", GetCreateEventData());
                                if (response != null && response.status == Constant.Status200 && response.response != null)
                                {
                                    ClearFields();
                                    //await App.Instance.Alert(Constant.EventUpdated, Constant.AlertTitle, Constant.Ok);
                                    ShowToast(Constant.AlertTitle, Constant.EventUpdated);
                                    IsUpdateEvent = false;
                                    Application.Current.MainPage = new NavigationPage(new MainPage());
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                                else if (response != null && response.status == Constant.Status401)
                                {
                                    SignOut();
                                    IsUpdateEvent = false;
                                    IsLoading = false;
                                }
                                else
                                {
                                    await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                    IsUpdateEvent = false;
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                            }
                            else
                            {
                                IsUpdateEvent = false;
                                TapCount = 0;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsUpdateEvent = false;
                            IsLoading = false;
                            TapCount = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }
        async Task UpdateBoostedEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                      App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (Validate())
                            {
                                IsLoading = true;
                                var response = await mainService.Put<ResultWrapperSingle<EventResponse>>(Constant.UpdateEventUrl + Convert.ToString(TappedEventId) + "/", GetCreateEventData());
                                if (response != null && response.status == Constant.Status200 && response.response != null)
                                {
                                    ClearFields();
                                    BoostEventConfirmation = false;
                                    if (IsUpdateEvent)
                                    {

                                        ShowToast(Constant.AlertTitle, Constant.EventUpdated);
                                        IsUpdateEvent = false;
                                    }
                                    Application.Current.MainPage = new NavigationPage(new MainPage());
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                                else if (response != null && response.status == Constant.Status401)
                                {
                                    SignOut();
                                    IsUpdateEvent = false;
                                    IsLoading = false;
                                }
                                else
                                {
                                    await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                    IsUpdateEvent = false;
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                            }
                            else
                            {
                                IsUpdateEvent = false;
                                TapCount = 0;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsUpdateEvent = false;
                            IsLoading = false;
                            TapCount = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }
        async Task UploadThumbnail()
        {
            if (SelectedMediaList != null && SelectedMediaList.Count > 0)
            {
                if (SelectedMediaList.Any(i => i.FileType == 1))
                {
                    var byteThumbnail = Device.RuntimePlatform == Device.Android ? App.DroidThumbnail : App.iOSImageThumbnail;
                    if (byteThumbnail != null && byteThumbnail.Length > 0)
                    {
                        var stream = new MemoryStream(byteThumbnail);
                        uniqueThumbName = Guid.NewGuid().ToString().Substring(0, 7) + "_thumb";
                        await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.event_videos_thumbnails, uniqueThumbName + Constant.AWS_File_Ext);
                    }
                }
            }
        }

        async Task UploadMedia()
        {
            await new AWSServices().UploadAWSFilePath(SelectedMediaList);
        }

        Events GetCreateEventData()
        {
            Events events = new Events();
            GetAddress();
            events.name = EventTitle;
            events.city = city;
            events.state = state;
            events.country = country;
            events.event_venue = EventVenue;
            events.location_address = EventLocation;
            var startDate = DateTime.Parse(EventFromDate.ToString("yyyy-MM-dd") + " " + EventFromTime);
            var endDate = DateTime.Parse(EventToDate.ToString("yyyy-MM-dd") + " " + EventToTime);
            var startDate2 = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDate2 = endDate.ToString("yyyy-MM-dd HH:mm:ss");
            events.event_start_date_time = startDate2;
            events.event_end_date_time = endDate2;
            events.time_zone_type = !string.IsNullOrEmpty(TimeZoneType) ? TimeZoneType : "MST";
            events.description = Description;
            events.latitude = eventLattitude;
            events.longitude = eventLogitude;
            events.is_boosted_event = IsBoostEvent;
            events.boosted_event_price = IsBoostEvent ? 5.0 : 0.0;
            events.is_public = isPublic;
            events.contact_number = string.IsNullOrEmpty(MobileNumber) ? null : MobileNumber;
            events.is_bottle_service = isBottleFeeChecked;
            events.is_free_time_event = isCoverFeeChecked;
            events.cover_fee_amount = isCoverFeeChecked ? Convert.ToDouble(CoverAmount) : 0.0;
            events.bottle_service_amount = isBottleFeeChecked ? Convert.ToDouble(BottleAmount) : 0.0;
            events.cover_photo = cover_Photo;
            List<Member> members = new List<Member>();
            List<Medium> medium = new List<Medium>();
            members.Clear();
            if (IsUpdateEvent)
            {
                if (UpdatedSelectedFriendsList != null && UpdatedSelectedFriendsList.Count > 0)
                {

                    foreach (var i in UpdatedSelectedFriendsList)
                    {
                        Member member = new Member();
                        member.user_id = i.friendId;
                        members.Add(member);

                    }
                }
            }
            else
            {
                if (SelectedFriendsList != null && SelectedFriendsList.Count > 0)
                {
                    foreach (var i in SelectedFriendsList)
                    {
                        Member member = new Member();
                        member.user_id = i.friendId;
                        members.Add(member);

                    }
                }
            }
            events.members = members;
            if (SelectedMediaList != null && SelectedMediaList.Count > 0)
            {
                foreach (var item in SelectedMediaList)
                {
                    Medium medi = new Medium();
                    medi.file_name = item.FileName;
                    medi.file_type = item.FileType;
                    medi.is_live = false;
                    if (medi.file_type == 1)
                    {
                        medi.file_thumbnail = uniqueThumbName + Constant.AWS_File_Ext;
                    }
                    medium.Add(medi);
                }
            }
            events.media = medium;
            return events;
        }

        void GetAddress()
        {
            var location = EventLocation.Split(',');
            if (location.Length > 0)
            {
                city = location[0].Trim();
                state = location.Length > 2 ? location[1].Trim() : location[0].Trim();
                country = location.Length > 2 ? location[2].Trim() : location.Length > 1 ? location[1].Trim() : location[0].Trim();
            }
        }
        public void ClearFields()
        {
            EventFromDate = DateTime.Now;
            EventToDate = DateTime.Now.AddDays(1);
            EventFromTime = DateTime.Now.AddHours(1).TimeOfDay;
            EventToTime = DateTime.Now.AddHours(2).TimeOfDay;
            isPublic = true;
            isCoverFeeChecked = false;
            isBottleFeeChecked = false;
            EventTitle = string.Empty;
            EventVenue = string.Empty;
            Description = string.Empty;
            city = string.Empty;
            state = string.Empty;
            country = string.Empty;
            SelectedFriendsList.Clear();
            CoverAmount = string.Empty;
            BottleAmount = string.Empty;
            TimeZoneType = string.Empty;
            ReasonToCancel = string.Empty;
            SelectedMediaList.Clear();
            IsUpdateEvent = false;
            IsAddressListVisible = false;
            MobileNumber = string.Empty;
        }
        public async Task<bool> GetLocBasedEventsList()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    return false;
                }
                else
                {
                    App.ShowMainPageLoader();
                    EventsList = new List<MyEventResponse>();
                    if (SessionManager.AccessToken != null && (pageNoLocBasedEvents == 1 || pageNoLocBasedEvents <= totalLocBasedPages))
                    {
                        tempocBasedEventOnMaps = new ObservableCollection<MyEvents>();
                        var url = string.Format(listURL, pageNoLocBasedEvents, eventLat, eventLong, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        var response = await mainService.Get<ResultWrapper<MyEventResponse>>(url);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                EventsList.Add(item);
                                string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
                                tempocBasedEventOnMaps.Add(new MyEvents { EventId = item.id, EventName = item.name, EventDateTime = date, Eventlat = item.latitude, EventLong = item.longitude });
                            }
                            totalLocBasedPages = GetPageCount(response.response[response.response.Count - 1].total_events);
                            return true;
                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            App.HideMainPageLoader();
                            return false;
                        }
                        else
                        {
                            App.HideMainPageLoader();
                            return false;
                        }
                    }
                    else
                    {
                        App.HideMainPageLoader();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                App.HideMainPageLoader();
                TapCount = 0;
                return false;
            }
        }
        public async Task<bool> GetMapLatLongBasedEventsList()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    return false;
                }
                else
                {
                    App.ShowMainPageLoader();
                    EventsList = new List<MyEventResponse>();
                    if (SessionManager.AccessToken != null && (pageNoLatLongBasedEvents == 1 || pageNoLatLongBasedEvents <= totalLocBasedPages))
                    {
                        temlatlongBasedEventOnMaps = new ObservableCollection<MyEvents>();
                        var url = string.Format(Constant.LatLongMapEventsUrl, pageNoLatLongBasedEvents, App.Latitude, App.Lognitude, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), FilterLocType);
                        var response = await mainService.Get<ResultWrapper<MyEventResponse>>(url);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                EventsList.Add(item);
                                string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
                                temlatlongBasedEventOnMaps.Add(new MyEvents { EventId = item.id, EventName = item.name, EventDateTime = date, Eventlat = item.latitude, EventLong = item.longitude });
                            }
                            totalLatlongBasedPages = GetPageCount(response.response[response.response.Count - 1].total_events);
                            return true;
                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            App.HideMainPageLoader();
                            return false;
                        }
                        else
                        {
                            App.HideMainPageLoader();
                            return false;
                        }
                    }
                    else
                    {
                        App.HideMainPageLoader();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                App.HideMainPageLoader();
                TapCount = 0;
                return false;
            }
        }
        public async Task GetLocEvents()
        {
            try
            {
                GetLocBasedEvents();
            }

            catch (Exception)
            {
                App.HideMainPageLoader();
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        public async void GetLocBasedEvents()
        {
            try
            {
                App.ShowMainPageLoader();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    bool isList = await GetLocBasedEventsList();
                    SetLocBasedEventsList(isList, EventsList);
                }
            }

            catch (Exception)
            {
                App.HideMainPageLoader();
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        public async Task GetMapLocBasedEvents()
        {
            try
            {
                App.ShowMainPageLoader();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    bool isList = await GetLocBasedEventsList();
                    SetLocBasedEventsList(isList, EventsList);
                }
            }

            catch (Exception)
            {
                App.HideMainPageLoader();
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        public async void GetMapLatLongEvents()
        {
            try
            {
                App.ShowMainPageLoader();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    bool isList = await GetMapLatLongBasedEventsList();
                    SetLatLongBasedEventsList(isList, EventsList);
                }
            }

            catch (Exception e)
            {
                App.HideMainPageLoader();
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        public async void GetNotificationList()
        {
            try
            {
                App.ShowMainPageLoader();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    bool isList = await GetNotifications();
                    SetNotificationList(isList, NotificationList);
                }
            }

            catch (Exception e)
            {
                App.HideMainPageLoader();
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        public async void GetMyEventsList()
        {
            try
            {
                IsLoading = true;
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    bool isList = await GetMyEvents();
                    SetEventsList(isList, EventsList);
                }
                IsLoading = false;
            }

            catch (Exception ex)
            {
                IsLoading = false;
                TapCount = 0;
                //await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        /// <summary>
        /// New Functionality
        /// </summary>
        public async void GetAllUpComingEvents()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return;
                }
                else
                {
                    if (SessionManager.AccessToken != null)
                    {
                        var AllEvents = new ObservableCollection<MyEvents>();
                        var response = await mainService.Get<ResultWrapper<MyEventResponse>>(Constant.AllUpcomingUrl + 1 + "&datetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            totalEventPages = GetPageCount(response.response[response.response.Count - 1].total_events);
                            //bool ishostedUpcoming = currentActiveEventType == MyEventType.Upcoming ? (FilterType.Equals(Constant.HostingText) ? true : false) : false;
                            IsEventListVisible = true;
                            IsNoEventVisible = false;
                            bool isEdit;
                            tempEventList = new ObservableCollection<MyEvents>();
                            foreach (var item in response.response)
                            {
                                float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 4;
                                string loc = item.event_venue + "," + item.location_address;
                                string attendee;
                                isEdit = false;
                                string date = SetEventDate(item.start_date, item.start_time);// + " " + item.time_zone_type;
                                bool isImageOneVisible;
                                bool isImageSecondVisible;
                                string imageOne;
                                string imageSecond;
                                if (item.event_attendees_count <= 0)
                                {
                                    attendee = "No attendees yet!";
                                    isImageOneVisible = false;
                                    isImageSecondVisible = false;
                                    imageOne = string.Empty;
                                    imageSecond = string.Empty;
                                }
                                else
                                {
                                    attendee = item.event_attendees_count > 2 ? "+" + Convert.ToString(item.event_attendees_count - 2) + " More attendees" : "No More attendees";
                                    if (item.event_attendees_count >= 2)
                                    {
                                        isImageOneVisible = true;
                                        isImageSecondVisible = true;
                                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                                        imageSecond = !string.IsNullOrEmpty(item.attendees[1].profile_image) ? PageHelper.GetUserImage(item.attendees[1].profile_image) : Constant.UserDefaultSquareImage;
                                    }
                                    else
                                    {
                                        isImageOneVisible = true;
                                        isImageSecondVisible = false;
                                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                                        imageSecond = string.Empty;
                                    }
                                }
                                EventFromDate = DateTime.Parse(item.start_date).Date;
                                EventToDate = DateTime.Parse(item.end_date).Date;
                                EventFromTime = DateTime.Parse(item.start_time).TimeOfDay;
                                EventToTime = DateTime.Parse(item.end_time).TimeOfDay;
                                if (currentActiveEventType == MyEventType.Upcoming)
                                {
                                    CheckInButtonStatus(item.event_status_label);
                                }
                                else
                                {
                                    IsCheckinButtonVisible = false;
                                }
                                var myEvent = new MyEvents();
                                myEvent.EventId = item.id;
                                if (!string.IsNullOrEmpty(item.profile_image))
                                    myEvent.profile_image = item.profile_image;
                                else
                                    myEvent.profile_image = Constant.ProfileIcon;
                                myEvent.EventName = item.name;
                                myEvent.StartDate = DateTime.Parse(item.start_date).Date;
                                myEvent.EventLikes = Convert.ToString(item.event_likes_count);
                                myEvent.EventAddress = loc;
                                myEvent.EventStatus = item.event_status_label;
                                myEvent.EventLitScore = Convert.ToString(item.event_lit_score) + Constant.LitScoreText;
                                myEvent.AttendeeCount = attendee;
                                myEvent.EventDateTime = date;
                                myEvent.StartDateTime = DateTime.Parse(item.start_date + " " + item.start_time);
                                myEvent.EndDateTime = DateTime.Parse(item.end_date + " " + item.end_time);
                                myEvent.IsEditIconVisible = isEdit;
                                myEvent.IsFirstImageVisible = isImageOneVisible;
                                myEvent.IsSecondImageVisible = isImageSecondVisible;
                                myEvent.AttendeeImageFirst = imageOne;
                                myEvent.AttendeeImageSecond = imageSecond;
                                myEvent.IsBoostEvent = item.is_boosted_event;
                                myEvent.IsCheckInButtonVisible = IsCheckinButtonVisible;
                                myEvent.IsShowViewAll = item.event_attendees_count > 0;
                                myEvent.is_star = item.is_star;
                                myEvent.cover_photo = item.cover_photo;
                                myEvent.ListBackColor = item.is_boosted_event ? Color.FromHex(Constant.BoostListBackColor) : Color.White;
                                tempEventList.Add(myEvent);
                            }
                            if(EventsList!=null)
                            EventsList.Clear();
                            foreach (var item in tempEventList)
                            {
                                item.PartyImage = string.IsNullOrEmpty(item.cover_photo) ? "party_story_header.png" : item.cover_photo;
                                if (item.is_star)
                                    item.IconStar = "iconStarred.png";
                                else
                                    item.IconStar = "iconStar.png";
                            }
                           // ListMyEvents = tempEventList;
                            AllUpcomingEvents = tempEventList;
                            pageNoMyEvents++;
                            IsLoading = false;
                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                TapCount = 0;
            }
        }

        async Task<bool> GetMyEvents()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (SessionManager.AccessToken != null && (pageNoMyEvents == 1 || pageNoMyEvents <= totalEventPages))
                    {
                        string url = "";
                        if (currentActiveEventType == MyEventType.Upcoming)
                        {
                            switch (FilterType)
                            {
                                case Constant.HostingText:
                                    url = Constant.HostedUpcomingUrl;
                                    break;
                                case Constant.AttendingText:
                                    url = Constant.AttendingUpcomingUrl;
                                    break;
                                case Constant.InterestedText:
                                    url = Constant.InterestedUpcomingUrl;
                                    break;
                                case Constant.CheckedInText:
                                    url = Constant.CheckedInUpcomingUrl;
                                    break;
                                default:
                                    url = Constant.AllUpcomingUrl;
                                    break;
                            }
                        }
                        else
                        {
                            switch (FilterType)
                            {
                                case Constant.HostedText:
                                    url = Constant.HostedPastUrl;
                                    break;
                                case Constant.AttendedText:
                                    url = Constant.AttendedPastUrl;
                                    break;
                                case Constant.CheckedInText:
                                    url = Constant.CheckedInPastUrl;
                                    break;
                                default:
                                    url = Constant.AllPastUrl;
                                    break;
                            }
                        }
                        EventsList = new List<MyEventResponse>();
                        var response = await mainService.Get<ResultWrapper<MyEventResponse>>(url + pageNoMyEvents + "&datetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            totalEventPages = GetPageCount(response.response[response.response.Count - 1].total_events);
                            foreach (var item in response.response)
                            {
                                EventsList.Add(item);
                            }
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        string SetEventDate(string startDate, string startTime)
        {
            var dateStart = DateTime.Parse(startDate + " " + startTime);
            return dateStart.Date.ToString("ddd,dd MMM").ToUpperInvariant() + ", " + dateStart.ToString("h:mm tt").Trim().ToUpperInvariant();
        }
        void SetEventsList(bool isList, List<MyEventResponse> list)
        {

            if (isList && pageNoMyEvents < 2)
            {
                SetEvents(list);
            }
            else if (isList)
            {
                SetEvents(list);
            }
            else if (!isList && pageNoMyEvents < 2)
            {
                IsEventListVisible = false;
                IsNoEventVisible = true;
                IsLoading = false;
            }
            else
            {
                ListMyEvents = tempEventList;
                AllUpcomingEvents = tempEventList;
                IsLoading = false;
            }
        }

        void SetEvents(List<MyEventResponse> list)
        {
            bool ishostedUpcoming = currentActiveEventType == MyEventType.Upcoming ? (FilterType.Equals(Constant.HostingText) ? true : false) : false;
            IsEventListVisible = true;
            IsNoEventVisible = false;
            bool isEdit;
            foreach (var item in list)
            {
                float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 4;
                string loc = item.event_venue + "," + item.location_address;
                string attendee;
                isEdit = false;
                string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
                bool isImageOneVisible;
                bool isImageSecondVisible;
                string imageOne;
                string imageSecond;
                if (ishostedUpcoming)
                {
                    if (DateTime.Parse(item.start_date).Date > DateTime.Now.Date)
                    {
                        isEdit = true;
                    }
                    else if (DateTime.Parse(item.start_date).Date == DateTime.Now.Date)
                    {
                        isEdit = DateTime.Now.AddHours(2).TimeOfDay <= DateTime.Parse(item.start_time).TimeOfDay ? true : false;
                    }
                    else
                    {
                        isEdit = false;
                    }
                }
                if (item.event_attendees_count <= 0)
                {
                    attendee = "No attendees yet!";
                    isImageOneVisible = false;
                    isImageSecondVisible = false;
                    imageOne = string.Empty;
                    imageSecond = string.Empty;
                }
                else
                {
                    attendee = item.event_attendees_count > 2 ? "+" + Convert.ToString(item.event_attendees_count - 2) + " More attendees" : "No More attendees";
                    if (item.event_attendees_count >= 2)
                    {
                        isImageOneVisible = true;
                        isImageSecondVisible = true;
                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                        imageSecond = !string.IsNullOrEmpty(item.attendees[1].profile_image) ? PageHelper.GetUserImage(item.attendees[1].profile_image) : Constant.UserDefaultSquareImage;
                    }
                    else
                    {
                        isImageOneVisible = true;
                        isImageSecondVisible = false;
                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                        imageSecond = string.Empty;
                    }
                }
                EventFromDate = DateTime.Parse(item.start_date).Date;
                EventToDate = DateTime.Parse(item.end_date).Date;
                EventFromTime = DateTime.Parse(item.start_time).TimeOfDay;
                EventToTime = DateTime.Parse(item.end_time).TimeOfDay;
                if (currentActiveEventType == MyEventType.Upcoming)
                {

                    CheckInButtonStatus(item.event_status_label);


                }
                else
                {
                    IsCheckinButtonVisible = false;
                }
                tempEventList.Add(new MyEvents
                {
                    EventId = item.id,
                    EventName = item.name,
                    EventLikes = Convert.ToString(item.event_likes_count),
                    EventAddress = loc,
                    EventStatus = item.event_status_label,
                    EventLitScore = Convert.ToString(item.event_lit_score) + Constant.LitScoreText,
                    AttendeeCount = attendee,
                    EventDateTime = date,
                    IsEditIconVisible = isEdit,
                    IsFirstImageVisible = isImageOneVisible,
                    IsSecondImageVisible = isImageSecondVisible,
                    AttendeeImageFirst = imageOne,
                    AttendeeImageSecond = imageSecond,
                    IsBoostEvent = item.is_boosted_event,
                    IsCheckInButtonVisible = IsCheckinButtonVisible,
                    IsShowViewAll = item.event_attendees_count > 0,
                    ListBackColor = item.is_boosted_event ? Color.FromHex(Constant.BoostListBackColor) : Color.White

                });
            }
            EventsList.Clear();
            ListMyEvents = tempEventList;
            AllUpcomingEvents = tempEventList;
            pageNoMyEvents++;
            IsLoading = false;
        }
        public void CheckInButtonStatus(string status)
        {
            switch (status)
            {
                case "Hosting":
                    IsCheckinButtonVisible = true;
                    break;
                case "Attending":
                    IsCheckinButtonVisible = true;
                    break;
                default:
                    IsCheckinButtonVisible = false;
                    break;
            }
        }
        public void SetLocBasedEventsList(bool isList, List<MyEventResponse> list)
        {
            if (isList && pageNoLocBasedEvents < 2)
            {
                SetLocEvents(list);
            }
            else if (isList)
            {
                SetLocEvents(list);
            }
            else if (!isList && pageNoLocBasedEvents < 2)
            {
                IsLocEventListVisible = false;
                //IsLocNoEventVisible = true;
                App.HideMainPageLoader();
            }
            else
            {
                ListLocBasedEvents = tempLocBasedEventList;
                App.HideMainPageLoader();
            }
        }
        public void SetLatLongBasedEventsList(bool isList, List<MyEventResponse> list)
        {
            if (isList && pageNoLatLongBasedEvents < 2)
            {
                SetLatLongEvents(list);
            }
            else if (isList)
            {
                SetLatLongEvents(list);
            }
            else if (!isList && pageNoLatLongBasedEvents < 2)
            {
                IsLocEventListVisible = false;
               // IsLocNoEventVisible = true;
                App.HideMainPageLoader();
            }
            else
            {
                ListLatLongBasedEvents = tempLatlongBasedEventList;
                App.HideMainPageLoader();
            }
        }
        public void SetNotificationList(bool isList, List<NotificationResponse> list)
        {
            if (isList && pageNoLocBasedEvents < 2)
            {
                SetNotification(list);
            }
            else if (isList)
            {
                SetNotification(list);
            }
            else if (!isList && pageNoLocBasedEvents < 2)
            {
                IsNotificationListVisible = false;
                IsNoNotificationVisible = true;
                App.HideMainPageLoader();
            }
            else
            {
                ListNotification = tmpNotificationList;
                App.HideMainPageLoader();
            }
        }
        void SetNotification(List<NotificationResponse> list)
        {
            IsNotificationListVisible = true;
            IsNoNotificationVisible = false;
            foreach (var item in list)
            {
                tmpNotificationList.Add(new NotificationResponse
                {
                    id = item.id,
                    message = item.message,
                    date = item.create_date.Date.ToString("ddd dd MMM, ") + item.create_date.ToString("h:mm tt").ToUpperInvariant(),
                    notification_unread_exist = item.notification_unread_exist,
                    title = item.title,
                    extra_data = item.extra_data,
                    profile_image= !string.IsNullOrEmpty(item.profile_image)?item.profile_image:Constant.UserDefaultSquareImage
                });
            }
            ListNotification = tmpNotificationList;
            if (ListNotification.Count == Constant.StatusZero)
            {
                IsNoNotificationVisible = true;
            }
            pageNoNotification++;
            App.HideMainPageLoader();
        }
        void SetLocEvents(List<MyEventResponse> list)
        {
            IsLocEventListVisible = true;
            IsLocNoEventVisible = false;

            foreach (var item in list)
            {
                float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 4;
                string loc = item.event_venue + "," + item.location_address;
                string attendee;
                string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
                bool isImageOneVisible;
                bool isImageSecondVisible;
                string imageOne;
                string imageSecond;
                if (item.event_attendees_count <= 0)
                {
                    attendee = "No attendees yet!";
                    isImageOneVisible = false;
                    isImageSecondVisible = false;
                    imageOne = string.Empty;
                    imageSecond = string.Empty;
                }
                else
                {
                    attendee = item.event_attendees_count > 2 ? "+" + Convert.ToString(item.event_attendees_count - 2) + " More attendees" : "No More attendees";
                    if (item.event_attendees_count >= 2)
                    {
                        isImageOneVisible = true;
                        isImageSecondVisible = true;
                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                        imageSecond = !string.IsNullOrEmpty(item.attendees[1].profile_image) ? PageHelper.GetUserImage(item.attendees[1].profile_image) : Constant.UserDefaultSquareImage;
                    }
                    else
                    {
                        isImageOneVisible = true;
                        isImageSecondVisible = false;
                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                        imageSecond = string.Empty;
                    }
                }
               
                CheckInButtonStatus(item.event_status_label);
                tempLocBasedEventList.Add(new MyEvents
                { 
                    EventId = item.id,
                    EventName = item.name,
                    EventLikes = Convert.ToString(item.event_likes_count),
                    EventAddress = loc,
                    EventLitScore = Convert.ToString(item.event_lit_score) + Constant.LitScoreText,
                    AttendeeCount = attendee,
                    EventDateTime = date,
                    IsEditIconVisible = false,
                    IsFirstImageVisible = isImageOneVisible,
                    IsSecondImageVisible = isImageSecondVisible,
                    AttendeeImageFirst = imageOne,
                    AttendeeImageSecond = imageSecond,
                    Eventlat = item.latitude,
                    EventLong = item.longitude,
                    IsBoostEvent = item.is_boosted_event,
                    IsCurrentLocation = false,
                    EventStatus = item.event_status_label,
                    LatLongEventsCount = item.lat_log_event_count,
                    IsCheckInButtonVisible = IsCheckinButtonVisible,
                    IsShowViewAll = item.event_attendees_count > 0,
                    ListBackColor = item.is_boosted_event ? Color.FromHex(Constant.BoostListBackColor) : Color.White
                    
            });
            }
            EventsList.Clear();
            List<MyEvents> filteredList = new List<MyEvents>();
            filteredList = tempLocBasedEventList.GroupBy(x => new { x.EventId }).Select(x => x.FirstOrDefault()).ToList();
            //ListLocBasedEvents = (ObservableCollection<Pulse.MyEvents>)tempLocBasedEventList.GroupBy(x => new { x.EventId }).Select(x => x.FirstOrDefault());
            if (tempLocBasedEventList != null)
                tempLocBasedEventList.Clear();
            int count = 1;
            foreach (var item in filteredList)
            {
                item.PartyImage = string.IsNullOrEmpty(item.cover_photo) ? "party_story_header.png" : item.cover_photo;
                if (item.is_star)
                    item.IconStar = "iconStarred.png";
                else
                    item.IconStar = "iconStar.png";
                tempLocBasedEventList.Add(item);
            }
            //AllUpcomingEvents= tempLocBasedEventList;
            if (AllUpcomingEvents.Count != 0)
                IsLocNoEventVisible = false;
            else
                IsLocNoEventVisible = true;
            ListLocBasedEvents = tempLocBasedEventList;
            pageNoLocBasedEvents++;
            App.HideMainPageLoader();
        }
        void SetLatLongEvents(List<MyEventResponse> list)
        {
            IsLocEventListVisible = true;
            IsLocNoEventVisible = false;

            foreach (var item in list)
            {
                float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 4;
                string loc = item.event_venue + "," + item.location_address;
                string attendee;
                //string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
                string date = SetEventDate(item.start_date, item.start_time);
                bool isImageOneVisible;
                bool isImageSecondVisible;
                string imageOne;
                string imageSecond;
                if (item.event_attendees_count <= 0)
                {
                    attendee = "No attendees yet!";
                    isImageOneVisible = false;
                    isImageSecondVisible = false;
                    imageOne = string.Empty;
                    imageSecond = string.Empty;
                }
                else
                {
                    attendee = item.event_attendees_count > 2 ? "+" + Convert.ToString(item.event_attendees_count - 2) + " More attendees" : "No More attendees";
                    if (item.event_attendees_count >= 2)
                    {
                        isImageOneVisible = true;
                        isImageSecondVisible = true;
                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                        imageSecond = !string.IsNullOrEmpty(item.attendees[1].profile_image) ? PageHelper.GetUserImage(item.attendees[1].profile_image) : Constant.UserDefaultSquareImage;
                    }
                    else
                    {
                        isImageOneVisible = true;
                        isImageSecondVisible = false;
                        imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
                        imageSecond = string.Empty;
                    }
                }
                CheckInButtonStatus(item.event_status_label);
                tempLatlongBasedEventList.Add(new MyEvents
                {
                    EventId = item.id,
                    EventName = item.name,
                    EventLikes = Convert.ToString(item.event_likes_count),
                    EventAddress = loc,
                    EventLitScore = Convert.ToString(item.event_lit_score) + Constant.LitScoreText,
                    AttendeeCount = attendee,
                    EventDateTime = date,
                    IsEditIconVisible = false,
                    IsFirstImageVisible = isImageOneVisible,
                    IsSecondImageVisible = isImageSecondVisible,
                    AttendeeImageFirst = imageOne,
                    AttendeeImageSecond = imageSecond,
                    Eventlat = item.latitude,
                    EventLong = item.longitude,
                    IsBoostEvent = item.is_boosted_event,
                    IsCurrentLocation = false,
                    EventStatus = item.event_status_label,
                    LatLongEventsCount = item.lat_log_event_count,
                    IsCheckInButtonVisible = IsCheckinButtonVisible,
                    IsShowViewAll = item.event_attendees_count > 0,
                    ListBackColor = item.is_boosted_event ? Color.FromHex(Constant.BoostListBackColor) : Color.White

                });
            }
            EventsList.Clear();
            List<MyEvents> filteredList = new List<MyEvents>();
            filteredList = tempLatlongBasedEventList.GroupBy(x => new { x.EventId }).Select(x => x.FirstOrDefault()).ToList();
            //ListLocBasedEvents = (ObservableCollection<Pulse.MyEvents>)tempLocBasedEventList.GroupBy(x => new { x.EventId }).Select(x => x.FirstOrDefault());
            if (tempLatlongBasedEventList != null)
                tempLatlongBasedEventList.Clear();

            foreach (var item in filteredList)
            {
                tempLatlongBasedEventList.Add(item);

            }
            ListLatLongBasedEvents = tempLatlongBasedEventList;
            pageNoLocBasedEvents++;
            App.HideMainPageLoader();
        }
        public async Task<bool> GetNotifications()
        {
            try
            {
                var uri = Constant.GetNotificationsUrl + pageNoNotification + "&datetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {

                    if (SessionManager.AccessToken != null)
                    {
                        NotificationList = new List<NotificationResponse>();
                        var response = await mainService.Get<ResultWrapperSingle<List<NotificationResponse>>>(Constant.GetNotificationsUrl + pageNoNotification + "&datetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (response != null && response.status == Constant.Status200 && response.response != null)
                        {
                            foreach (var item in response.response)
                            {
                                NotificationList.Add(item);
                            }
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                return false;
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                return false;
            }

        }
        public async Task<bool> MarkReadNotification()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {

                    if (SessionManager.AccessToken != null)
                    {
                        var response = await mainService.Put<MarkReadResponse>(Constant.MarkReadNotificationsUrl, null);
                        if (response != null && response.status == Constant.Status200 && response.response != null)
                        {
                            foreach (var item in response.response.notification)
                            {
                                IsNotification = false;
                                if (item.Contains("Notifications updated"))
                                {
                                    return true;
                                }
                            }
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                return false;
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                return false;

            }

        }
        public async Task<bool> GetUnreadNotificationCount()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {

                    if (SessionManager.AccessToken != null)
                    {
                        var response = await mainService.Get<NotificationCountResponse<UnreadNotificationcountResponse>>(Constant.UnreadNotificationsUrl);
                        if (response != null && response.status == Constant.Status200 && response.Response != null)
                        {
                            IsNotification = response.Response.unread_notification_count > Constant.StatusZero;
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                TapCount = 0;
                return false;
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                TapCount = 0;
                return false;

            }

        }
        public async Task FetchEventDetail(string id, bool isDetailShown)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (SessionManager.AccessToken != null)
                    {
                        var response = await mainService.Get<ResultWrapperSingle<EventDetailsResponse>>(Constant.EventDetailsUrl + id + "/");
                        if (response != null && response.status == Constant.Status200 && response.response != null)
                        {
                            TappedEventId = response.response.id;
                            IsAdmin = response.response.is_superuser;
                            EventTitle = response.response.name;
                            IsOwner = response.response.is_owner;
                            IconStar = response.response.is_star ? "iconStarred.png" : "iconStar.png";
                            CoverPhoto= string.IsNullOrEmpty(response.response.cover_photo)? "party_story_header.png": response.response.cover_photo;
                            IsPartyTextVisible = CoverPhoto == "party_story_header.png" ? true : false;
                            EventLocation = response.response.location_address;
                            EventVenue = response.response.event_venue;
                            Description = response.response.description;
                            IsGoing = response.response.is_going;
                            EventFromDate = DateTime.Parse(response.response.start_date).Date;
                            EventToDate = DateTime.Parse(response.response.end_date).Date;
                            EventFromTime = DateTime.Parse(response.response.start_time).TimeOfDay;
                            EventToTime = DateTime.Parse(response.response.end_time).TimeOfDay;
                            MobileNumber = response.response.contact_number;
                            IsReportedSpam = response.response.reported_spam;
                            TimeZoneType = response.response.time_zone_type;
                            eventDateOnMap = SetEventDate(response.response.start_date, response.response.start_time);// + " " + response.response.time_zone_type;
                            eventLogitude = response.response.longitude;
                            eventLattitude = response.response.latitude;
                            EventLitScore = Convert.ToString(response.response.event_lit_score) + Constant.LitScoreText;
                            IsCoverFreeAmountAvailable = Convert.ToDouble(response.response.cover_fee_amount) > 0.0 ? true : false;
                            IsCoverAmount = response.response.is_free_time_event;
                            EventCoverAmount = string.Empty;
                            EventBottleAmount = string.Empty;
                            IsUploadCoverImageVisible = SessionManager.UserId == response.response.user ? true : false;
                            IsUserCheckedIn = response.response.is_checkin;
                            IsBoostEvent = response.response.is_boosted_event;
                            IsNotAlreadyBoosted = !IsBoostEvent;
                            IsUserNotCheckedIn = !response.response.is_checkin;
                            IsContactNumberAvailable = !string.IsNullOrEmpty(response.response.contact_number);
                            if (!string.IsNullOrEmpty(response.response.cover_fee_amount) && IsCoverAmount)
                            {
                                response.response.cover_fee_amount = response.response.cover_fee_amount.Remove(response.response.cover_fee_amount.Length - 3);
                                EventCoverAmount = response.response.cover_fee_amount;
                            }
                            IsBottleAmount = response.response.is_bottle_service;
                            if (!string.IsNullOrEmpty(response.response.bottle_service_amount) && IsBottleAmount)
                            {
                                EventBottleAmount = response.response.bottle_service_amount.Remove(response.response.bottle_service_amount.Length - 3);
                            }
                            CoverFreeAmountText = Constant.PaidEventText + response.response.cover_fee_amount + Constant.CoverFeeText;
                            numberLikes = response.response.event_likes_count;
                            is_like = response.response.is_like;
                            HostedName = response.response.host_name;
                            HostImage = !string.IsNullOrEmpty(response.response.host_profile_image) ? PageHelper.GetUserImage(response.response.host_profile_image) : Constant.UserDefaultSquareImage;
                            EventDate = eventDateOnMap;
                            EventTime = SetEventDate(response.response.end_date, response.response.end_time);// + " " + response.response.time_zone_type;
                            //LocationEvent = EventVenue + ", " + EventLocation;
                            LocationEvent = EventVenue;
                            SetLikeCount();
                            if (response.response.total_comments > 0)
                            {
                                double d = Convert.ToDouble(response.response.total_comments) / 10.0;
                                totalPagesComment = Convert.ToInt32(Math.Ceiling(d));
                            }
                            else
                            {
                                totalPagesComment = 0;
                            }
                            transactionEmail = !string.IsNullOrEmpty(response.response.transaction_email) ? response.response.transaction_email : string.Empty;
                            FetchEventAttendees(response.response.attendees);
                            FetchMedia(response.response.event_media);
                            FetchComment(response.response.comments);

                         
                            IsCheckinDisableButtonVisible = false;
                            IsAddressListVisible = false;
                            IsSearchItemSelected = true;
                            IsEditIconOnDetailVisible = false;
                           
                            if (currentActiveEventType == MyEventType.Upcoming)
                            {
                                if (EventToDate.Date == DateTime.Now.Date && EventToTime < DateTime.Now.TimeOfDay)
                                {
                                    IsJoinButtonVisible = false;
                                    IsStackOptionVisible = false;
                                    IsCheckinButtonVisible = true; 
                                }
                                else
                                {
                                    if (!response.response.is_owner)
                                    {
                                        IsJoinButtonVisible = true;
                                        IsStackOptionVisible = true;
                                        IsCheckinButtonVisible = false;
                                        if (string.IsNullOrEmpty(response.response.event_invitee_status))
                                        {
                                            SetDefault();
                                        }
                                        else
                                        {
                                            switch (response.response.event_invitee_status)
                                            {
                                                case "1":
                                                    IsInterestedImageVisible = false;
                                                    IsNotInterestedImageVisible = true;
                                                    IsInterestedIconVisible = true;
                                                    IsNotInterestedIconVisible = false;
                                                    IsJoinButtonVisible = true;
                                                    InterestedTextColor = Color.FromHex(Constant.FilterLightGreyColor);
                                                    NotInterestedTextColor = Color.FromHex(Constant.PinkButtonColor);
                                                    break;
                                                case "2":
                                                    IsInterestedIconVisible = false;
                                                    IsNotInterestedIconVisible = true;
                                                    IsInterestedImageVisible = true;
                                                    IsNotInterestedImageVisible = false;
                                                    IsJoinButtonVisible = true;
                                                    InterestedTextColor = Color.FromHex(Constant.PinkButtonColor);
                                                    NotInterestedTextColor = Color.FromHex(Constant.FilterLightGreyColor);
                                                    break;
                                                case "3":
                                                    IsJoinButtonVisible = false;
                                                    IsStackOptionVisible = false;
                                                    IsCheckinButtonVisible = true;
                                                    break;
                                                default:
                                                    SetDefault();
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        IsJoinButtonVisible = false;
                                        IsStackOptionVisible = false;
                                        if (DateTime.Parse(response.response.start_date).Date > DateTime.Now.Date)
                                        {
                                            IsEditIconOnDetailVisible = true;
                                        }
                                        else if (DateTime.Parse(response.response.start_date).Date == DateTime.Now.Date)
                                        {
                                            IsEditIconOnDetailVisible = DateTime.Now.AddHours(2).TimeOfDay <= DateTime.Parse(response.response.start_time).TimeOfDay ? true : false;
                                        }
                                        else
                                        {
                                            IsEditIconOnDetailVisible = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                IsJoinButtonVisible = false;
                                IsStackOptionVisible = false;
                                IsCheckinButtonVisible = true;
                            }
                            var startDateTime = EventFromDate.Date + EventFromTime;
                            var EventDates = DateTime.Compare(DateTime.Now, startDateTime);
                            //for upcoming events
                            if (EventDates < 0)
                                CheckinButtonText = response.response.is_going ? Constant.JoinedGuestListText : Constant.JoinGuestListText;
                            //for live events
                            else if(EventDates > 0 || EventDates == 0)
                                CheckinButtonText = response.response.is_checkin ? Constant.CheckedInText : Constant.CheckInText;
                            IsCheckinButtonVisible = true;
                            IsEndEventVisible = IsOwner ? true : false;
                            IsJoinButtonVisible = IsCoverFreeAmountAvailable ? true : false;
                            EventPaidButtonText = response.response.is_paid ? Constant.ViewTicketText : Constant.JoinText;
                            if (isDetailShown)
                                await Navigation.PushModalAsync(new EventDetailPage(id));
                            IsLoading = false;
                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                        }
                    }
                    else
                    {
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception e)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }

        void FetchComment(List<CommentResponse> comments)
        {
            if (comments != null && comments.Count > 0)
            {
                IsStackCommentVisible = true;
                CommenteeName = comments[0].user_info.fullname;
                CommentText = comments[0].comment_text;
                CommentDate = DateTime.Parse(comments[0].send_date).ToLocalTime().ToString("ddd,dd MMM, h:mm tt");
                CommentId = Convert.ToString(comments[0].id);
                IsCommentDeleteIconVisible = comments[0].is_owner ? true : false;
                CommenteeImage = !string.IsNullOrEmpty(comments[0].user_info.profile_image) ? PageHelper.GetUserImage(comments[0].user_info.profile_image) : Constant.UserDefaultSquareImage;
            }
            else
            {
                IsStackCommentVisible = false;
            }
        }

        void FetchMedia(List<EventMedia> event_media)
        {
            EventMediaList.Clear();
            if (event_media != null && event_media.Count > 0)
            {
                foreach (var i in event_media)
                {
                    EventMediaList.Add(i);
                }
            }
        }

        void FetchEventAttendees(List<Attendee> attendees)
        {
            EventAttendeeList.Clear();
            if (attendees != null && attendees.Count > 0)
            {
                foreach (var i in attendees)
                {
                    EventAttendeeList.Add(i);
                }
                AttendeeCountLabel = attendees.Count.ToString() + " Guests Invited";
            }
            else
            {
                AttendeeCountLabel = "No Guests Invited";
            }
        }

        void SetDefault()
        {
            InterestedTextColor = Color.FromHex(Constant.FilterLightGreyColor);
            NotInterestedTextColor = Color.FromHex(Constant.FilterLightGreyColor);
            IsInterestedImageVisible = false;
            IsNotInterestedImageVisible = true;
            IsInterestedIconVisible = false;
            IsNotInterestedIconVisible = true;
        }

        public async Task EventAttendingConfirmation(string type)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            IsLoading = true;
                            EventAttend eventAttend = new EventAttend();
                            eventAttend.invitee_status = eventStatus;
                            var response = await mainService.Put<ResultWrapperSingle<EventConfirmation>>(Constant.EventAttendingConfirmationUrl + Convert.ToString(TappedEventId) + "/", eventAttend);
                            if (response != null && response.status == Constant.Status200 && response.response != null)
                            {
                                if (type == "Detail")
                                {
                                    ClearFields();
                                    await FetchEventDetail(Convert.ToString(TappedEventId), false);
                                }
                                else
                                {
                                    ShowToast(Constant.AlertTitle, "Event joined successfully.");
                                    IsLoading = false;
                                    TapCount = 0;
                                }
                                TapCount = 0;
                            }
                            else if (response != null && response.status == Constant.Status401)
                            {
                                SignOut();
                                IsLoading = false;
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                IsLoading = false;
                                TapCount = 0;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsLoading = false;
                            TapCount = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
            }
        }

        public async void LikeEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    IsLoading = true;
                    EventMedia eventmed = new EventMedia();
                    eventmed.event_id = TappedEventId;
                    var response = await mainService.Post<ResultWrapperSingle<LikesResponse>>(Constant.LikeEventUrl, eventmed);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        if (response.response.likes_count > numberLikes)
                        {
                            numberLikes++;
                            IsLikedIconVisible = true;
                            IsLikeIconVisible = false;
                            LabelLikeColor = Color.FromHex(Constant.PinkButtonColor);

                        }
                        else
                        {
                            numberLikes--;
                            IsLikedIconVisible = false;
                            IsLikeIconVisible = true;
                            LabelLikeColor = Color.FromHex(Constant.FilterLightGreyColor);
                            if (numberLikes <= 0)
                            {
                                LikesCount = numberLikes.ToString() + Constant.LikeText;
                                IsLoading = false;
                                return;
                            }
                        }
                        LikesCount = numberLikes > 0 && numberLikes < 2 ? numberLikes.ToString() + Constant.LikeText : numberLikes.ToString() + Constant.LikesText;
                        IsLoading = false;
                    }
                    else
                    {
                        IsLoading = false;
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception)
            {
                IsLoading = false;
            }
        }

        public async Task ShareEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    IsLoading = true;
                    TagFriend friend = new TagFriend();
                    var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ShareEventUrl + TappedEventId + "/", friend);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        IsLoading = false;
                    }
                    else
                    {
                        IsLoading = false;
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception)
            {
                IsLoading = false;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);

            }
        }

        public async Task ReportEvent()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    IsLoading = true;
                    var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ReportEventUrl, GetEventReportData(EvenetReportedData));
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        App.Instance.Alert(Constant.ReportMessage, Constant.AlertTitle, Constant.Ok);
                        IsLoading = false;
                    }
                    else
                    {
                        IsLoading = false;
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception)
            {
                IsLoading = false;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);

            }
        }

        public async Task ReportEventComment()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    IsLoading = true;
                    var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ReportEventCommentUrl, GetEventCommentReportData(EvenetCommentReportedData));
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        App.Instance.Alert(Constant.ReportMessage, Constant.AlertTitle, Constant.Ok);
                        IsLoading = false;
                    }
                    else
                    {
                        IsLoading = false;
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                    }
                }
            }
            catch (Exception)
            {
                IsLoading = false;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);

            }
        }

        public async void GetGuests()
        {
            try
            {
                IsLoading = true;
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {

                    bool isList = await GetGuestList();
                    SetGuestList(isList, GuestList);

                }
                IsLoading = false;
            }

            catch (Exception)
            {
                IsLoading = false;
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }

        async Task<bool> GetGuestList()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (SessionManager.AccessToken != null && (pageNoGuests == 1 || pageNoGuests <= totalGuestPages))
                    {
                        string url = "";

                        switch (currentActiveGuestType)
                        {
                            case GuestType.Invited:
                                url = Constant.InvitedGuestUrl;
                                break;
                            case GuestType.Interested:
                                url = Constant.InterestedGuestUrl;
                                break;
                            case GuestType.NotInterested:
                                url = Constant.NotInterestedGuestUrl;
                                break;
                            case GuestType.CheckedIn:
                                url = Constant.CheckedInGuestUrl;
                                break;
                            default:
                                url = Constant.AttendeesGuestUrl;
                                break;
                        }
                        GuestList = new List<GuestResponse>();
                        var response = await mainService.Get<ResultWrapper<GuestResponse>>(url + TappedEventId + "/?page=" + pageNoGuests);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                GuestList.Add(item);
                            }
                            totalGuestPages = GetPageCount(response.response[response.response.Count - 1].total_attendees);
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        void SetGuestList(bool isList, List<GuestResponse> list)
        {

            if (isList && pageNoGuests < 2)
            {
                SetGuests(list);
            }
            else if (isList)
            {
                SetGuests(list);
            }
            else if (!isList && pageNoGuests < 2)
            {
                IsListGuestVisible = false;
                IsNoGuestFoundVisible = true;
                IsLoading = false;
            }
            else
            {
                ListGuests = tempGuestList;
                IsLoading = false;
            }
        }

        void SetGuests(List<GuestResponse> list)
        {
            IsListGuestVisible = true;
            IsNoGuestFoundVisible = false;
            foreach (var item in list)
            {
                float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
                string mutual = item.user_info.mutual_user_count <= 0 ? "No" : Convert.ToString(item.user_info.mutual_user_count);
                bool isAddFriendvisible = item.user_info.request_type == Constant.AddFriendText ? true : false;
                bool isFriendsvisible = item.user_info.request_type == Constant.FriendText ? true : false;
                bool iscancelRequestvisible = item.user_info.request_type == Constant.CancelRequestText ? true : false;
                bool isconfirmRequestvisible = item.user_info.request_type == Constant.ConfirmRequestText ? true : false;
                if (SessionManager.UserId == item.user_info.id)
                {
                    isAddFriendvisible = false;
                    isFriendsvisible = false;
                    iscancelRequestvisible = false;
                    isconfirmRequestvisible = false;
                }
                tempGuestList.Add(new Friend { friendId = item.user_info.id, cornerRadius = cornerradius, friendUsername = item.user_info.username, friendFullname = item.user_info.fullname, friendPic = string.IsNullOrEmpty(item.user_info.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.user_info.profile_image), friendMutual = mutual + " Mutual Friends", IsAddFriendButtonVisible = isAddFriendvisible, IsFriendsButtonVisible = isFriendsvisible, IsCancelRequestButtonVisible = iscancelRequestvisible, IsConfirmRequestButtonVisible = isconfirmRequestvisible });
            }
            GuestList.Clear();
            ListGuests = tempGuestList;
            pageNoGuests++;
            IsLoading = false;
        }

        public async Task AddFriend(int friendId)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (TapCount < 1)
                    {
                        TapCount = 1;
                        IsLoading = true;
                        if (!string.IsNullOrEmpty(SessionManager.AccessToken))
                        {
                            FriendRequest friend = new FriendRequest();
                            friend.friend_id = friendId;
                            var result = await mainService.Post<ResultWrapperSingle<AddFriendResponse>>(Constant.AddFriendUrl, friend);
                            if (result != null && result.status == Constant.Status200)
                            {
                                tempGuestList.Clear();
                                pageNoGuests = 1;
                                totalGuestPages = 1;
                                GetGuests();
                                TapCount = 0;

                            }
                            else if (result != null && result.status == Constant.Status111 && result.message != null && result.message.friend_id != null && result.message.friend_id.Count > 0)
                            {
                                await App.Instance.Alert(result.message.friend_id[0], Constant.AlertTitle, Constant.Ok);
                                tempGuestList.Clear();
                                pageNoGuests = 1;
                                totalGuestPages = 1;
                                GetGuests();
                                TapCount = 0;
                            }
                            else if (result != null && result.status == Constant.Status401)
                            {
                                SignOut();
                                IsLoading = false;
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                TapCount = 0;
                                IsLoading = false;
                            }
                        }
                        else
                        {
                            IsLoading = false;
                            TapCount = 0;
                        }

                    }
                    else
                    {
                        await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                        TapCount = 0;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                TapCount = 0;
                IsLoading = false;
            }
        }

        public async Task ChangeRequestStatus(int friendId, FriendType friendType, string pageType)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (TapCount < 1)
                    {
                        TapCount = 1;
                        IsLoading = true;
                        if (!string.IsNullOrEmpty(SessionManager.AccessToken))
                        {
                            FriendRequest friend = new FriendRequest();
                            if (pageType == Constant.SearchText)
                            {
                                friend.friend_id = friendType == FriendType.CancelRequest ? friendId : SessionManager.UserId;
                                friend.user_id = friendType == FriendType.CancelRequest ? SessionManager.UserId : friendId;
                            }
                            else
                            {
                                friend.friend_id = SessionManager.UserId;
                                friend.user_id = friendId;
                            }
                            friend.friend_request_status = friendType == FriendType.CancelRequest ? 2 : 1;
                            var result = await mainService.Put<ResultWrapperSingle<AddFriendResponse>>(Constant.RequestChangeUrl, friend);
                            if (result != null && result.status == Constant.Status200)
                            {
                                tempGuestList.Clear();
                                pageNoGuests = 1;
                                totalGuestPages = 1;
                                GetGuests();
                                TapCount = 0;
                            }
                            else if (result != null && result.status == Constant.Status111 && result.message != null && result.message.non_field_errors != null && result.message.non_field_errors.Count > 0)
                            {
                                await App.Instance.Alert(result.message.non_field_errors[0], Constant.AlertTitle, Constant.Ok);
                                tempGuestList.Clear();
                                pageNoGuests = 1;
                                totalGuestPages = 1;
                                GetGuests();
                                TapCount = 0;
                            }
                            else if (result != null && result.status == Constant.Status401)
                            {
                                SignOut();
                                IsLoading = false;
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                TapCount = 0;
                                IsLoading = false;
                            }
                        }
                        else
                        {
                            IsLoading = false;
                            TapCount = 0;
                        }

                    }
                    else
                    {
                        await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                        TapCount = 0;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                TapCount = 0;
                IsLoading = false;
            }
        }


        public async Task<bool> PostComment()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (!string.IsNullOrEmpty(CommentToPost.Trim()))
                            {
                                IsLoading = true;

                                var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.SetCommentUrl, GetCommentData());
                                if (response != null && response.status == Constant.Status200 && response.response != null)
                                {

                                    IsLoading = false;
                                    TapCount = 0;
                                    return true;
                                }
                                else if (response != null && response.status == Constant.Status401)
                                {
                                    SignOut();
                                    IsLoading = false;
                                    return false;
                                }
                                else
                                {
                                    await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                    IsLoading = false;
                                    TapCount = 0;
                                    return false;
                                }
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.CommentRequiredMessage, Constant.AlertTitle, Constant.Ok);
                                TapCount = 0;
                                return false;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsLoading = false;
                            TapCount = 0;
                            return false;
                        }
                    }
                    else
                    {
                        IsLoading = false;
                        TapCount = 0;
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
                return false;
            }
        }

        Comments GetCommentData()
        {
            Comments comments = new Comments();
            comments.comment_text = CommentToPost;
            comments.event_id = TappedEventId;
            var sendDate = (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+" + TimeZoneInfo.Local.BaseUtcOffset).Replace("+-", "-");
            comments.send_date = sendDate.Remove(sendDate.Length - 3);

            List<TagFriend> tags = new List<TagFriend>();
            if (SelectedFriendsList != null && SelectedFriendsList.Count > 0)
            {
                foreach (var i in SelectedFriendsList)
                {
                    TagFriend tag = new TagFriend();
                    tag.friend_id = i.friendId;
                    tags.Add(tag);
                }
            }
            comments.tag_friends = tags;
            return comments;
        }

        EventReport GetEventReportData(string comment)
        {
            EventReport reportData = new EventReport();
            reportData.event_id = TappedEventId;
            reportData.reason_to_spam = comment;
            return reportData;
        }
        EventCommentReport GetEventCommentReportData(string comment)
        {
            EventCommentReport reportData = new EventCommentReport();
            reportData.event_id = TappedEventId;
            reportData.event_comment_id = Convert.ToInt32(TappedCommentId);
            reportData.reason_to_spam = comment;
            return reportData;
        }

        public async Task<bool> GetCommentList()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (SessionManager.AccessToken != null)
                    {
                        CommentList = new List<CommentResponse>();
                        var response = await mainService.Get<ResultWrapper<CommentResponse>>(Constant.GetAllCommentsUrl + TappedEventId + "/?page=" + pageNoComment);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                CommentList.Add(item);
                            }
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> DeleteComment()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            TapCount++;
                            IsLoading = true;
                            DeleteComment comment = new DeleteComment();
                            var response = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.DeleteCommenturl + TappedCommentId + "/", comment);
                            if (response != null && response.status == Constant.Status200 && response.response != null)
                            {
                                IsLoading = false;
                                TapCount = 0;
                                return true;
                            }
                            else if (response != null && response.status == Constant.Status401)
                            {
                                TapCount = 0;
                                SignOut();
                                IsLoading = false;
                                return false;
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                IsLoading = false;
                                TapCount = 0;
                                return false;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsLoading = false;
                            TapCount = 0;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
                return false;
            }
        }

        public async Task<bool> GetMediaList(bool isLive)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (SessionManager.AccessToken != null && (pageNoMedia == 1 || pageNoMedia <= totalLiveMediaPages))
                    {
                        MediaList = new List<EventMedia>();
                        string url = isLive ? Constant.EventLiveMediaListUrl : Constant.EventMediaListUrl;
                        var response = await mainService.Get<ResultWrapper<EventMedia>>(url + TappedEventId + "/?page=" + pageNoMedia);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                MediaList.Add(item);
                            }
                            totalLiveMediaPages = GetPageCount(response.response[response.response.Count - 1].total_media);
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> GetMyMediaList()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (SessionManager.AccessToken != null && (pageNoMedia == 1 || pageNoMedia <= totalMediaPages))
                    {
                        MediaList = new List<EventMedia>();
                        var response = await mainService.Get<ResultWrapper<EventMedia>>(Constant.UserMediaUrl + pageNoMedia);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                MediaList.Add(item);
                            }
                            totalMediaPages = GetPageCount(response.response[response.response.Count - 1].total_media);
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        IsRefreshing = true;
                        pageNoLocBasedEvents = 1;
                       // totalLocBasedPages = 1;
                        tempLocBasedEventList.Clear();
                        GetAllUpComingEvents();
                        IsRefreshing = false;
                    }
                    else
                    {
                        await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    }
                });
            }
        }
        public ICommand NotificationsRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        IsRefreshing = true;
                        pageNoLocBasedEvents = 1;
                        // totalLocBasedPages = 1;
                        tempLocBasedEventList.Clear();
                        bool isList = await GetNotifications();
                        SetNotificationList(isList, NotificationList);
                        // GetNotificationList();
                        IsRefreshing = false;
                    }
                    else
                    {
                        await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    }
                });
            }
        }
        public bool CardDetailsValidate()
        {
            if (string.IsNullOrEmpty(CardNumber))
            {
                App.Instance.Alert(Constant.CardNumberRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (CardNumber.Replace(" ", "").Length < 16)
            {
                App.Instance.Alert(Constant.CardNumberLength, Constant.AlertTitle, Constant.Ok);
                return false;
            }

            else if (!Regex.IsMatch(CardNumber.Replace(" ", ""), Constant.InvalidNumericRegex))
            {
                App.Instance.Alert(Constant.InvalidCardNumber, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (string.IsNullOrEmpty(CheckCardType()))
            {
                App.Instance.Alert(Constant.CardTypeNotSupported, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!isMonthSelected)
            {
                App.Instance.Alert(Constant.ExpiryMonthRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }

            else if (!isYearSelected)
            {
                App.Instance.Alert(Constant.ExpiryYearRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }

            else if (ExpiryYear.Equals(Convert.ToString(DateTime.Now.Year)) && ExpiryMonth < DateTime.Now.Month)
            {
                App.Instance.Alert("Invalid expiry date", Constant.AlertTitle, Constant.Ok);
                return false;
            }

            else if (string.IsNullOrEmpty(CVV))
            {
                App.Instance.Alert(Constant.CVVRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (CVV.Length < 3)
            {
                App.Instance.Alert(Constant.InvalidCVV, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!Regex.IsMatch(CVV, Constant.InvalidNumericRegex))
            {
                App.Instance.Alert(Constant.InvalidCVV, Constant.AlertTitle, Constant.Ok);
                return false;
            }

            else if (string.IsNullOrEmpty(NameOnCard))
            {
                App.Instance.Alert(Constant.NameOnCardReuired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (Regex.IsMatch(NameOnCard, Constant.InvalidStringRegex))
            {
                App.Instance.Alert(Constant.InvalidNameOnCard, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (NameOnCard.Split(' ').Length <= 1)
            {
                App.Instance.Alert(Constant.InvalidNameOnCard, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (NameOnCard.Split(' ').Length > 1 && string.IsNullOrEmpty(NameOnCard.Split(' ')[1]))
            {
                App.Instance.Alert(Constant.InvalidNameOnCard, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!CountryList.Any(x => x.name.Trim().ToLower() == EventBillingCountry.Trim().ToLower()))
            {
                App.Instance.Alert(Constant.CountryRequired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (string.IsNullOrEmpty(EventBillingAddress))
            {
                App.Instance.Alert(Constant.BillingAddressReuired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (Regex.IsMatch(EventBillingAddress, Constant.InvalidStringRegex))
            {
                App.Instance.Alert(Constant.InvalidAddress, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (string.IsNullOrEmpty(EventBillingCity))
            {
                App.Instance.Alert(Constant.BillingCityReuired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (Regex.IsMatch(EventBillingCity, Constant.InvalidStringRegex))
            {
                App.Instance.Alert(Constant.InvalidCity, Constant.AlertTitle, Constant.Ok);
                return false;
            }

            else if (string.IsNullOrEmpty(EventBillingZipCode))
            {
                App.Instance.Alert(Constant.BillingZipReuired, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else if (!Regex.IsMatch(EventBillingZipCode, Constant.AlphaNumericRegex))
            {
                App.Instance.Alert(Constant.InvalidZip, Constant.AlertTitle, Constant.Ok);
                return false;
            }
            else
            {
                return true;
            }
        }
        public void ClearPaymentFields()
        {
            NameOnCard = string.Empty;
            CardNumber = string.Empty;
            CVV = string.Empty;
            EventBillingCity = string.Empty;
            EventBillingState = string.Empty;
            EventBillingAddress = string.Empty;
            EventBillingZipCode = string.Empty;
        }
        async void MakePayment()
        {

            if (IsBoostEvent)
            {

                if (IsJoinEvent)
                {
                    await MakeNormalPayment();
                }
                else
                {
                    if (!IsUpdateEvent)
                    {
                        SelectedFriendsList.Clear();
                        UpdatedSelectedFriendsList.Clear();
                    }
                    if (string.IsNullOrEmpty(PaymentEmail))
                    {
                        await App.Instance.Alert("Please enter your email first", Constant.AlertTitle, Constant.Ok);
                    }
                    else if (!string.IsNullOrEmpty(transactionEmail) && !PaymentEmail.Equals(transactionEmail))
                    {
                        await App.Instance.Alert("Please verify your email first", Constant.AlertTitle, Constant.Ok);
                    }
                    else if (IsEmailVerified)
                    {
                        //SessionManager.Email = PaymentEmail;
                        await MakeBoostedEventPayment();
                    }
                    else
                    {
                        await App.Instance.Alert("Please verify your email first", Constant.AlertTitle, Constant.Ok);

                    }
                }
            }
            else
            {
                await MakeNormalPayment();
            }

        }
        async Task MakeBoostedEventPayment()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (CardDetailsValidate())
                            {
                                IsLoading = true;
                                TapCount++;
                                var paymentData = await new PaymentService().MakePayment(PreparePayment());
                                if (paymentData != null && paymentData.state == "approved")
                                {

                                    MyTransaction myTransaction = new MyTransaction();
                                    myTransaction.transaction_id = paymentData.id;
                                    myTransaction.email = PaymentEmail;
                                    myTransaction.status = 0;
                                    myTransaction.is_bottle_service = isUserTakenBottleService;
                                    myTransaction.total_amount = TotalAmount;
                                    myTransaction.payment_type = PaymentType.BOSSTEDPAYMENT;
                                    var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.PaymentSuccessfulApi + TappedEventId + "/", myTransaction);
                                    if (response != null && response.status == Constant.Status200)
                                    {
                                        //eventStatus = 3;
                                        //EventAttend eventAttend = new EventAttend();
                                        //eventAttend.invitee_status = eventStatus;
                                        //var result = await mainService.Put<ResultWrapperSingle<EventConfirmation>>(Constant.EventAttendingConfirmationUrl + Convert.ToString(TappedEventId) + "/", eventAttend);
                                        //if (result != null && result.status == Constant.Status200 && result.response != null)
                                        //{
                                        ShowToast(Constant.AlertTitle, Constant.PaymentSuccessfulMessage);
                                        ClearPaymentFields();
                                        TapCount = 0;
                                        await UpdateBoostedEvent();
                                        IsLoading = false;
                                        //}
                                    }
                                    else
                                    {
                                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                        TapCount = 0;
                                        IsLoading = false;
                                    }
                                }
                                else
                                {
                                    await App.Instance.Alert(Constant.PaymentDeclinedMessage, Constant.AlertTitle, Constant.Ok);
                                    TapCount = 0;
                                    IsLoading = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                TapCount = 0;
                IsLoading = false;
            }
        }
        async Task MakeNormalPayment()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            if (CardDetailsValidate())
                            {
                                IsLoading = true;
                                TapCount++;
                                var paymentData = await new PaymentService().MakePayment(PreparePayment());
                                if (paymentData != null && paymentData.state == "approved")
                                {

                                    MyTransaction myTransaction = new MyTransaction();
                                    myTransaction.transaction_id = paymentData.id;
                                    myTransaction.email = PaymentEmail;
                                    myTransaction.status = 0;
                                    myTransaction.is_bottle_service = isUserTakenBottleService;
                                    myTransaction.total_amount = TotalAmount;
                                    myTransaction.payment_type = PaymentType.JOININGPAYMENT;
                                    myTransaction.card_no = CardNumber;
                                    myTransaction.name = NameOnCard;
                                    myTransaction.expiry_month = ExpiryMonth;
                                    myTransaction.expiry_year = Convert.ToInt32(ExpiryYear);
                                    myTransaction.country = EventBillingCountry;
                                    myTransaction.city = EventBillingCity;
                                    myTransaction.state = EventBillingState;
                                    myTransaction.address = EventBillingAddress;
                                    myTransaction.postal_code = EventBillingZipCode;
                                    var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.PaymentSuccessfulApi + TappedEventId + "/", myTransaction);
                                    if (response != null && response.status == Constant.Status200)
                                    {
                                        eventStatus = 3;
                                        EventAttend eventAttend = new EventAttend();
                                        eventAttend.invitee_status = eventStatus;
                                        var result = await mainService.Put<ResultWrapperSingle<EventConfirmation>>(Constant.EventAttendingConfirmationUrl + Convert.ToString(TappedEventId) + "/", eventAttend);
                                        if (result != null && result.status == Constant.Status200 && result.response != null)
                                        {
                                            ShowToast(Constant.AlertTitle, Constant.PaymentSuccessfulMessage);
                                            //await App.Instance.Alert(Constant.PaymentSuccessfulMessage, Constant.AlertTitle, Constant.Ok);
                                            IsJoinEvent = false;
                                            ClearPaymentFields();
                                            Application.Current.MainPage = new NavigationPage(new MainPage());
                                            TapCount = 0;
                                            IsLoading = false;
                                        }
                                    }
                                    else
                                    {
                                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                        TapCount = 0;
                                        IsLoading = false;
                                    }
                                }
                                else
                                {
                                    await App.Instance.Alert(Constant.PaymentDeclinedMessage, Constant.AlertTitle, Constant.Ok);
                                    TapCount = 0;
                                    IsLoading = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                TapCount = 0;
                IsLoading = false;
            }
        }
        PaymentDetail PreparePayment()
        {
            string firstName = "";
            string LastName = "";
            string[] Names = NameOnCard.Split(' ');
            if (Names.Length > 1)
            {
                firstName = Names[0];
                LastName = Names[1];
            }
            else
            {
                firstName = Names[0].ToUpper();
            }

            Details otherAmountDetail = new Details
            {
                subtotal = Convert.ToString(TotalAmount),
            };
            Amount amountdetail = new Amount
            {
                total = Convert.ToString(TotalAmount),
                currency = Constant.CurrencyType,
                details = otherAmountDetail,
            };

            Transaction transactionDetail = new Transaction
            {
                amount = amountdetail,
                description = Constant.TransactionDescriptionText,
            };

            List<Transaction> transactionsList = new List<Transaction>();
            transactionsList.Add(transactionDetail);
            var countryCodeDetail = CountryList.Where(x => x.name.Trim().ToLower() == EventBillingCountry.Trim().ToLower()).FirstOrDefault();
            BillingAddress billingAddressDetail = new BillingAddress
            {
                line1 = EventBillingAddress,
                city = EventBillingCity,
                postal_code = EventBillingZipCode,
                country_code = countryCodeDetail.iso,
                state = EventBillingState
            };

            CreditCard creditCardDetail = new CreditCard
            {
                number = CardNumber.Replace(" ", ""),
                type = CheckCardType(),
                expire_month = Convert.ToString(ExpiryMonth),
                expire_year = Convert.ToInt32(ExpiryYear),
                cvv2 = CVV,
                first_name = firstName,
                last_name = LastName,
                billing_address = billingAddressDetail,
            };
            FundingInstrument fundingInstrumentDetail = new FundingInstrument();
            fundingInstrumentDetail.credit_card = creditCardDetail;
            List<FundingInstrument> fundingInstruments = new List<FundingInstrument>();
            fundingInstruments.Add(fundingInstrumentDetail);

            Payer payerDetail = new Payer
            {
                payment_method = Constant.PaymentMethod,
                funding_instruments = fundingInstruments,
            };

            PaymentDetail paymentDetail = new PaymentDetail
            {
                intent = Constant.PaymentIntent,
                payer = payerDetail,
                transactions = transactionsList,
            };
            return paymentDetail;
         }

        public string CheckCardType()
        {
            if (Regex.IsMatch(CardNumber.Replace(" ", ""), Constant.VisaCardRegex))
            {
                return (Constant.VisaCardType);
            }
            else if (Regex.IsMatch(CardNumber.Replace(" ", ""), Constant.MasterCardRegex))
            {
                return (Constant.MasterCardType);
            }
            else if (Regex.IsMatch(CardNumber.Replace(" ", ""), Constant.AmericanExpressCardRegex))
            {
                return (Constant.AmericanExpressCardType);
            }
            else if (Regex.IsMatch(CardNumber.Replace(" ", ""), Constant.DiscoverCardRegex))
            {
                return (Constant.DiscoverCardType);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteMedia(string classId)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            TapCount++;
                            IsLoading = true;
                            DeleteComment comment = new DeleteComment();
                            var response = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.MediaDeleteUrl + classId + "/", comment);
                            if (response != null && response.status == Constant.Status200 && response.response != null)
                            {
                                IsLoading = false;
                                TapCount = 0;
                                return true;
                            }
                            else if (response != null && response.status == Constant.Status401)
                            {
                                TapCount = 0;
                                SignOut();
                                IsLoading = false;
                                return false;
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                IsLoading = false;
                                TapCount = 0;
                                return false;
                            }
                        }
                        else
                        {
                            await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                            IsLoading = false;
                            TapCount = 0;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
                return false;
            }
        }

        public async Task UpdateUserLocation()
        {
            Device.StartTimer(new TimeSpan(0, 0, 60), () =>
            {
                // do something every 600 seconds
                Device.BeginInvokeOnMainThread(async() => 
                {
                  await  PostUserLocation();
                });
                return true; // runs again, or false to stop
            });
        }
        public async Task PostUserLocation()
        {
            try
            {
                UserLocationModel locationModel = new UserLocationModel();
                if(!string.IsNullOrEmpty(SessionManager.Email))
                {
                    locationModel.email = SessionManager.Email;
                    locationModel.latitude = double.Parse(eventLat);
                    locationModel.longitude = double.Parse(eventLong);
                    var res = await mainService.Put<PostUserLocationResponse>(Constant.PostUserLocationUrl, locationModel);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        #endregion

    }
}