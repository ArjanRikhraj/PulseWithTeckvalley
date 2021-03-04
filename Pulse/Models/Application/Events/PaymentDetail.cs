using System.Collections.Generic;

namespace Pulse
{
	public class PaypalToken
	{
		public string scope { get; set; }
		public string nonce { get; set; }
		public string access_token { get; set; }
		public string token_type { get; set; }
		public string app_id { get; set; }
		public long expires_in { get; set; }
	}

	public class PaymentDetail
	{
		public string intent { get; set; }
		public Payer payer { get; set; }
		public List<Transaction> transactions { get; set; }
	}

	public class BillingAddress
	{
		public string line1 { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string postal_code { get; set; }
		public string country_code { get; set; }
	}

	public class CreditCard
	{
		public string id { get; set; }
		public string number { get; set; }
		public string type { get; set; }
		public string expire_month { get; set; }
		public int expire_year { get; set; }
		public string cvv2 { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string payer_id { get; set; }
		public BillingAddress billing_address { get; set; }
	}

	public class SaveCreditCard
	{
		public string number { get; set; }
		public string type { get; set; }
		public string expire_month { get; set; }
		public int expire_year { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string external_customer_id { get; set; }
	}

	public class FundingInstrument
	{
		public CreditCard credit_card { get; set; }
		public CreditCardToken credit_card_token { get; set; }
	}

	public class CreditCardToken
	{
		public string credit_card_id { get; set; }
		public string payer_id { get; set; }
	}

	public class Payer
	{
		public string payment_method { get; set; }
		public List<FundingInstrument> funding_instruments { get; set; }
	}

	public class Details
	{
		public string subtotal { get; set; }
		public string tax { get; set; }
		public string shipping { get; set; }
	}

	public class Amount
	{
		public string total { get; set; }
		public string currency { get; set; }
		public Details details { get; set; }
	}

	public class Amount2
	{
		public string total { get; set; }
		public string currency { get; set; }
	}

	public class Link
	{
		public string href { get; set; }
		public string rel { get; set; }
		public string method { get; set; }
	}

	public class FmfDetails
	{
	}

	public class ProcessorResponse
	{
		public string avs_code { get; set; }
		public string cvv_code { get; set; }
	}

	public class Sale
	{
		public string id { get; set; }
		public string create_time { get; set; }
		public string update_time { get; set; }
		public Amount2 amount { get; set; }
		public string state { get; set; }
		public string parent_payment { get; set; }
		public List<Link> links { get; set; }
		public FmfDetails fmf_details { get; set; }
		public ProcessorResponse processor_response { get; set; }
	}

	public class RelatedResource
	{
		public Sale sale { get; set; }
	}

	public class Transaction
	{
		public Amount amount { get; set; }
		public string description { get; set; }
		public List<RelatedResource> related_resources { get; set; }
	}

	public class Link2
	{
		public string href { get; set; }
		public string rel { get; set; }
		public string method { get; set; }
	}

	public class PaymentResponse
	{
		public string id { get; set; }
		public string create_time { get; set; }
		public string update_time { get; set; }
		public string state { get; set; }
		public string intent { get; set; }
		public Payer payer { get; set; }
		public List<Transaction> transactions { get; set; }
		public List<Link2> links { get; set; }
	}

	public class CreditCardsResponse
	{
		public List<CreditCard> items { get; set; }
	}
}

