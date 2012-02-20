task :default => :compile

task :clean do
  sh "rm -rf BraintreeEncryption.Library/bin"
  sh "rm -rf BraintreeEncryption.Library/obj"
  sh "rm -rf BraintreeEncryption.Library.Tests/bin"
  sh "rm -rf BraintreeEncryption.Library.Tests/obj"
end

task :compile => :clean do
  sh "msbuild BraintreeEncryption.Library.sln"
end
