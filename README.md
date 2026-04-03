# Introduction

This project is meant to be the foundation for PvZ2 rewrite with backwards compatibility for PvZ2 modding. 

Initially, he goal of this project is to reverse engineer PvZ2 and to build a new engine that is compatible with PvZ2 mods. However, It is very much too complex for me as I would soon find out. Hence, this project studies and uses the groundwork that amazing people before have built to unpack, modify and repack modding resources and my own decompilation work.

Many names such as "ResourceBinary" are not the official name. I came up with it to make developement "easier" by avoiding excessive use of acronym. Decompilation of PvZ2lib.so creates hints to what the file do, and what it is really called in developements.
# Credits
Big thanks to:
- __*YingFengTingYu*__ for the amazing resource that is [PopStudio_Old](https://github.com/YingFengTingYu/PopStudio_Old).

# Other resources
- __*h3x4n1um*__ [RETON](https://github.com/h3x4n1um/RETON).
- __*nineteendo*__ [pyvz2](https://github.com/nineteendo/pyvz2).

# PvZ2 Data
## Summary
All PvZ2 core data (levels, worlds, ui) are stored in the `.obb` file. `.obb` (Opaque Binary Blob) uses [ResourceBinary (Rsb)](./PopLoader/DataProcessor/Rsb/) file format and hence shall be called Rsb file. ResourceBinary is an archival file format, storing files and create grouping (seperate from directory) to aid in loading content. File and directory name in Rsb file is case insensitive.
Some file formats Rsb uses are: Rton and Json are file formats to store object data (zombie types, plant, level, ...) and are interchangable. Pam is Popcap Animation format. Ptx is Popcap raw Texture format. Bnk and Wem are AudioKenetics proprietary sound formats. Xml, Ttf, Txt you can easily look it up.

## An extremely rough outline of the loading process
First PvZ2 load from properties\resources.rton
The it load configs from cdn_configs.json forceupdateconfig.json.
Then from the package directory, PvZ2 loads abunch of other files:
award_config.rton, arcade_config.rton, magento_filter.rton, products.rton, calendar_config.rton, calendar_schedule.rton, calendar_themes.rton, thymed_event_schedule.rton, 
thymed_event_track_definitions.rton, market_schedule.rton, market_layout.rton, plant_rv_rental_schedule.rton, playersegments.rton, pinatacoreloot.rton, PowerupTypes.rton, liveconfig.rton, LootModifierSchedule.rton, easquared_config.rton, LevelGen.rton, ZombieSwapLists.rton, ZombieSwapProperties.rton, News.rton, PlantLevels.rton, PlantMastery.rton, PlantPowerUps.rton, Quests.rton, quest_themes.rton, lod_config.rton, lod_events.rton, lod_holidays.rton, lod_rewards.rton, joust_config.rton, joust_schedule.rton, joust_season_schedule.rton, joust_leaderboards.rton, joust_progressiverewards.rton, joust_tournamentrewards.rton, joust_crownrewards.rton, joust_levels.rton, joust_profile.rton, joust_season_rewards.rton, rift_config.rton, rift_event_config.rton, rift_perk_progression.rton, rift_schedule.rton, rift_sub_event.rton, rift_zomboss_rewards.rton, rift_perks.rton, rift_level_unlocks.rton, rift_first_clear_rewards.rton, 

_saveheader.rton
save_%s.rton, cheatTearaways.rton, _activequests.rton, 