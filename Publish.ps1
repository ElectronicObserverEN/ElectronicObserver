$configuration = "Release"
$arch = "x64"
$os = "win"
$framework = "net8.0-windows7"
$selfContained = "false"
$output = "..\ElectronicObserver\bin\x64\publish\"

cd .\Browser\
dotnet publish --configuration $configuration --arch $arch --os $os --framework $framework --self-contained $selfContained --output $output
cd ..
cd .\ElectronicObserver\
dotnet publish --configuration $configuration --arch $arch --os $os --framework $framework --self-contained $selfContained --output $output
cd ..

# it's hard to make this work for previews
# cd .\bin
# cd .\x64
# 
# Compress-Archive publish\*.* -DestinationPath publish.zip
# 
# cd ..
# cd ..