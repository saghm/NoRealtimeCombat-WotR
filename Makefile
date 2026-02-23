.PHONY: clean

all: build

build: Main.cs NoRealtimeCombat.csproj
	dotnet build -c Release

clean:
	rm -rf bin obj build out

zip: build Info.json
	mkdir -p out
	zip -j out/NoRealtimeCombat.zip Info.json bin/Release/net48/NoRealtimeCombat.{dll,pdb}
