using Blazored.LocalStorage;
using JumpRace.Blazor.Models;

namespace JumpRace.Blazor.Services;

public class ConfigService
{
    private readonly ILocalStorageService _storage;
    private const string StorageKey = "jumpRaceConfigV11";

    public ConfigService(ILocalStorageService storage)
    {
        _storage = storage;
    }

    public async Task<GameConfig> LoadAsync()
    {
        try
        {
            var config = await _storage.GetItemAsync<GameConfig>(StorageKey);
            if (config != null && config.Players.Count >= 2)
            {
                // Force 2 lanes
                config.Players = config.Players.Take(2).ToList();
                return config;
            }
        }
        catch
        {
            // ignore and return default
        }

        return CreateDefault();
    }

    public async Task SaveAsync(GameConfig config)
    {
        await _storage.SetItemAsync(StorageKey, config);
    }

    public async Task ClearAsync()
    {
        await _storage.RemoveItemAsync(StorageKey);
    }

    public static GameConfig CreateDefault()
    {
        return new GameConfig();
    }
}
