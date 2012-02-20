# Braintree Windows Phone Encryption

This library is for use with [Braintree's payment gateway](http://braintreepayments.com/) in concert with one of [the supported client libraries](http://braintreepayments.com/docs).  It encrypts sensitive payment information using the public key of an asymmetric key pair.

## Getting Started

There are a couple of ways to use the Windows Phone client encryption library.

### Assembly File

1. Simply download the assembly file from this repo and reference it to your Windows Phone project.

### Windows Phone Class Library

1. Clone this repo.
2. Import the Braintree Windows Phone class library into your solution.
3. Add the library as a project dependency and make sure to reference the library.

Here's a quick example.

Configure the library to use your public key.

```csharp
var braintree = new Braintree("YOUR_CLIENT_SIDE_PUBLIC_ENCRYPTION_KEY");
```

And call the `Encrypt` method passing in the data you wish to be encrypted.

```csharp
var encryptedValue = braintree.Encrypt("sensitiveValue");
```

Because we are using asymmetric encryption, you will be unable to decrypt the data you have encrypted using your public encryption key. Only the Braintree Gateway will be able to decrypt these encrypted values.  This means that `encryptedValue` is now safe to pass through your server to be used in the Server-to-Server API of one of our client libraries.

## Retrieving your Encryption Key

When Client-Side encryption is enabled for your Braintree Gateway account, a key pair is generated and you are given a specially formatted version of the public key.

## Encrypting Form Values

The normal use case for this library is to encrypt a credit card number and CVV code before a form is submitted to your server.  A simple example of this in Windows Phone might look something like this:

```csharp
namespace BraintreeEncryptionExample
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            DisableForm();
            PostToMerchantServer();
        }

        private void PostToMerchantServer()
        {
            var braintree = new Braintree(PublicKey);
            var parameters = new Dictionary<string, object>
                                 {
                                     {"cc_number", braintree.Encrypt(CreditCardNumber.Text)},
                                     {"cc_exp_date", braintree.Encrypt(ExpirationDate.Text)},
                                     {"cc_cvv", braintree.Encrypt(Cvv.Text)}
                                 };

            var client = new BraintreeHttpClient(_merchantServerUrl, UploadStringCompleted);
            client.Post(parameters);
        }
    }
}
```
