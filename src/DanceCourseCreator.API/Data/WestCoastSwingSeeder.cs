using DanceCourseCreator.API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace DanceCourseCreator.API.Data;

public static class WestCoastSwingSeeder
{
    /// <summary>
    /// Seeds the database with West Coast Swing test data including patterns, exercises, lessons, and complete courses.
    /// This method is idempotent and can be run multiple times without creating duplicates.
    /// </summary>
    public static async Task SeedAsync(DanceCourseDbContext context)
    {
        // Check if data already exists - skip seeding if so
        var hasData = await context.Patterns.AnyAsync() || 
                      await context.Lessons.AnyAsync() || 
                      await context.Courses.AnyAsync();
        
        if (hasData)
        {
            // Data already seeded, skip
            return;
        }
        
        // Ensure we have an instructor user for creating the data
        var instructorId = await EnsureInstructorUserAsync(context);
        
        // Seed patterns (dance moves)
        var patternIds = await SeedPatternsAsync(context, instructorId);
        
        // Seed exercises (technique exercises)
        var exerciseIds = await SeedExercisesAsync(context, instructorId);
        
        // Save patterns and exercises before creating lessons
        await context.SaveChangesAsync();
        
        // Seed lessons for first course (8 x 90 min)
        var lessonIds = await SeedLessonsAsync(context, instructorId, patternIds, exerciseIds);
        
        // Seed lessons for extended course (12 x 60 min)
        var extendedLessonIds = await SeedExtendedLessonsAsync(context, instructorId, patternIds, exerciseIds);
        
        // Save lessons before creating courses
        await context.SaveChangesAsync();
        
        // Seed first course
        await SeedCourseAsync(context, instructorId, lessonIds);
        
        // Seed extended course (12 sessions of 1 hour each)
        await SeedExtendedCourseAsync(context, instructorId, extendedLessonIds);
        
        await context.SaveChangesAsync();
    }
    
    private static async Task<string> EnsureInstructorUserAsync(DanceCourseDbContext context)
    {
        const string instructorEmail = "instructor@dancecourse.com";
        var instructor = await context.Users.FirstOrDefaultAsync(u => u.Email == instructorEmail);
        
        if (instructor == null)
        {
            instructor = new User
            {
                Id = "wcs-instructor-id",
                Name = "WCS Instruktör",
                Email = instructorEmail,
                Role = UserRole.Instructor,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("instructor123"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Users.Add(instructor);
            await context.SaveChangesAsync();
        }
        
        return instructor.Id;
    }
    
    private static async Task<Dictionary<string, string>> SeedPatternsAsync(DanceCourseDbContext context, string creatorId)
    {
        var patterns = new Dictionary<string, PatternOrExercise>
        {
            ["left-side-pass"] = new PatternOrExercise
            {
                Id = "pattern-left-side-pass",
                Type = PatternType.Pattern,
                Name = "Left Side Pass",
                Description = "En av de mest grundläggande turerna i West Coast Swing där followern passerar leaderens vänstra sida",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Linear slot movement",
                EstimatedMinutes = 15,
                BpmRangeMin = 90,
                BpmRangeMax = 110,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Walk, Walk, Anchor",
                    "Follower: Walk, Walk, Walk, Walk, Anchor"
                },
                Counts = new List<string> { "1, 2, 3&4, 5&6" },
                TeachingPoints = new List<string>
                {
                    "Fokusera på tydlig connection",
                    "Anchor step på plats",
                    "Followern håller tension i slot"
                },
                CommonMistakes = new List<string>
                {
                    "För svag connection under pass",
                    "Glömmer anchor step",
                    "För snabb tempo"
                },
                Tags = new List<string> { "grundtur", "slot", "anchor" }
            },
            ["underarm-turn"] = new PatternOrExercise
            {
                Id = "pattern-underarm-turn",
                Type = PatternType.Pattern,
                Name = "Underarm Turn",
                Description = "En klassisk rotation där followern gör en turn under leaderens arm",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Linear with rotation",
                EstimatedMinutes = 15,
                BpmRangeMin = 90,
                BpmRangeMax = 110,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Walk, Walk, raise arm, Anchor",
                    "Follower: Walk, Walk, Turn under arm, Anchor"
                },
                Counts = new List<string> { "1, 2, 3&4, 5&6" },
                Rotations = new List<string> { "360° turn för follower" },
                Prerequisites = new List<string> { "Left Side Pass" },
                TeachingPoints = new List<string>
                {
                    "Armen skapar en 'dörr' för followern",
                    "Tydlig lead på count 3",
                    "Bibehåll connection genom turn"
                },
                CommonMistakes = new List<string>
                {
                    "För tidig eller sen rotation",
                    "Tappade connection",
                    "För hög arm under turn"
                },
                Tags = new List<string> { "grundtur", "rotation", "underarm" }
            },
            ["sugar-push"] = new PatternOrExercise
            {
                Id = "pattern-sugar-push",
                Type = PatternType.Pattern,
                Name = "Sugar Push",
                Description = "Fundamentalt push-pull mönster som lärs ut i början för att förstå compression och connection",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Push-pull in slot",
                EstimatedMinutes = 20,
                BpmRangeMin = 85,
                BpmRangeMax = 105,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Step back, replace, walk walk, anchor",
                    "Follower: Walk walk, replace, walk walk, anchor"
                },
                Counts = new List<string> { "1, 2, 3, 4, 5&6" },
                TeachingPoints = new List<string>
                {
                    "Compression på count 1-2",
                    "Push-connection på 3-4",
                    "Anchor tillsammans"
                },
                CommonMistakes = new List<string>
                {
                    "För stark compression",
                    "Missar connection på push",
                    "Fel timing på anchor"
                },
                Tags = new List<string> { "grundtur", "compression", "push-pull" }
            },
            ["starter-step"] = new PatternOrExercise
            {
                Id = "pattern-starter-step",
                Type = PatternType.Pattern,
                Name = "Starter Step",
                Description = "Grundläggande starter för att komma igång med West Coast Swing",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Entry into slot",
                EstimatedMinutes = 10,
                BpmRangeMin = 90,
                BpmRangeMax = 110,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Rock step, triple step, walk walk",
                    "Follower: Walk walk, triple step, walk walk"
                },
                Counts = new List<string> { "1, 2, 3&4, 5, 6" },
                TeachingPoints = new List<string>
                {
                    "Soft rock step",
                    "Triple step på plats",
                    "Smooth transition till walk"
                },
                Tags = new List<string> { "grundtur", "starter" }
            },
            ["whip"] = new PatternOrExercise
            {
                Id = "pattern-whip",
                Type = PatternType.Pattern,
                Name = "Whip",
                Description = "En signaturtur i WCS där followern gör en whip-action och kommer tillbaka",
                Level = DanceLevel.Improver,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Full slot with whip action",
                EstimatedMinutes = 20,
                BpmRangeMin = 90,
                BpmRangeMax = 105,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Walk, walk, anchor with direction change",
                    "Follower: Walk, walk, walk, whip action, return, anchor"
                },
                Counts = new List<string> { "1, 2, 3&4, 5&6" },
                Prerequisites = new List<string> { "Left Side Pass", "Anchor Step" },
                TeachingPoints = new List<string>
                {
                    "Tydlig direction change från leader",
                    "Follower: whip från bröstkorgen",
                    "Return till slot",
                    "Strong anchor"
                },
                CommonMistakes = new List<string>
                {
                    "För tidig whip action",
                    "Tappar slot-orientation",
                    "För svag anchor"
                },
                Tags = new List<string> { "intermediate", "whip", "slot" }
            },
            ["whip-inside-turn"] = new PatternOrExercise
            {
                Id = "pattern-whip-inside-turn",
                Type = PatternType.Pattern,
                Name = "Whip with Inside Turn",
                Description = "Whip med added rotation - followern gör en inside turn under whip",
                Level = DanceLevel.Intermediate,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Full slot with rotation",
                EstimatedMinutes = 20,
                BpmRangeMin = 85,
                BpmRangeMax = 100,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Walk, walk, lead inside turn, anchor",
                    "Follower: Walk, walk, walk, inside turn during whip, return, anchor"
                },
                Counts = new List<string> { "1, 2, 3&4, 5&6" },
                Rotations = new List<string> { "180-360° inside turn" },
                Prerequisites = new List<string> { "Whip", "Underarm Turn" },
                TeachingPoints = new List<string>
                {
                    "Lead turn på count 3",
                    "Follower maintains compression",
                    "Turn och whip samtidigt",
                    "Finish with solid anchor"
                },
                Tags = new List<string> { "intermediate", "whip", "rotation" }
            },
            ["tuck-turn"] = new PatternOrExercise
            {
                Id = "pattern-tuck-turn",
                Type = PatternType.Pattern,
                Name = "Tuck Turn",
                Description = "En turn där followern 'tuckas in' till en tight rotation",
                Level = DanceLevel.Improver,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Rotation into slot",
                EstimatedMinutes = 20,
                BpmRangeMin = 85,
                BpmRangeMax = 105,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Walk, tuck lead, anchor",
                    "Follower: Walk, walk, tight turn, walk, anchor"
                },
                Counts = new List<string> { "1, 2, 3&4, 5&6" },
                Rotations = new List<string> { "270-360° tight turn" },
                Prerequisites = new List<string> { "Left Side Pass", "Underarm Turn" },
                TeachingPoints = new List<string>
                {
                    "Tight turn, close to leader",
                    "Strong frame under rotation",
                    "Exit cleanly till slot"
                },
                CommonMistakes = new List<string>
                {
                    "För vid turn",
                    "Lost balance",
                    "Unclear direction"
                },
                Tags = new List<string> { "rotation", "tuck", "improver" }
            },
            ["basket-whip"] = new PatternOrExercise
            {
                Id = "pattern-basket-whip",
                Type = PatternType.Pattern,
                Name = "Basket Whip",
                Description = "En mer avancerad variant där båda dansare går in i en 'basket' position innan whip",
                Level = DanceLevel.Intermediate,
                DanceStyle = DanceStyle.WestCoastSwing,
                Slot = "Basket position into whip",
                EstimatedMinutes = 25,
                BpmRangeMin = 80,
                BpmRangeMax = 100,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Walk, walk into basket, lead whip, anchor",
                    "Follower: Walk, walk into basket, whip action, return, anchor"
                },
                Counts = new List<string> { "1, 2, 3&4, 5&6, 7&8" },
                Prerequisites = new List<string> { "Whip", "Frame & Connection" },
                TeachingPoints = new List<string>
                {
                    "Båda dansare går into basket tillsammans",
                    "Maintain strong frame i basket",
                    "Clean exit till whip",
                    "Return till slot"
                },
                CommonMistakes = new List<string>
                {
                    "Oklart lead in i basket",
                    "Tappar connection",
                    "För tidig whip"
                },
                Tags = new List<string> { "advanced", "basket", "whip" }
            }
        };
        
        var result = new Dictionary<string, string>();
        
        foreach (var kvp in patterns)
        {
            var existing = await context.Patterns.FirstOrDefaultAsync(p => p.Id == kvp.Value.Id);
            if (existing == null)
            {
                context.Patterns.Add(kvp.Value);
            }
            result[kvp.Key] = kvp.Value.Id;
        }
        
        return result;
    }
    
    private static async Task<Dictionary<string, string>> SeedExercisesAsync(DanceCourseDbContext context, string creatorId)
    {
        var exercises = new Dictionary<string, PatternOrExercise>
        {
            ["frame-connection"] = new PatternOrExercise
            {
                Id = "exercise-frame-connection",
                Type = PatternType.Exercise,
                Name = "Frame & Connection",
                Description = "Grundläggande övning för att utveckla rätt frame och connection mellan partners",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                EstimatedMinutes = 15,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Hitta neutral frame position",
                    "Öva compression och stretch",
                    "Känn connection utan att gripa",
                    "Öva med olika hand holds"
                },
                TeachingPoints = new List<string>
                {
                    "Frame kommer från core, inte armar",
                    "Soft hands, strong frame",
                    "Bibehåll connection genom movement",
                    "Frame adjusts till follower's height"
                },
                CommonMistakes = new List<string>
                {
                    "Stiff arms",
                    "Gripping med händerna",
                    "Collapsed frame",
                    "För mycket armmuskel"
                },
                Tags = new List<string> { "teknik", "frame", "connection", "grund" }
            },
            ["anchor-step"] = new PatternOrExercise
            {
                Id = "exercise-anchor-step",
                Type = PatternType.Exercise,
                Name = "Anchor Step",
                Description = "Träna den karakteristiska anchor step som är fundamental i WCS",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                EstimatedMinutes = 15,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Step back med ena foten",
                    "Replace weight framåt",
                    "Känn 'brake' och balance"
                },
                Counts = new List<string> { "5&6" },
                TeachingPoints = new List<string>
                {
                    "Anchor är ett 'brake', inte bara steg",
                    "Weight transfer tydligt",
                    "Håll frame under anchor",
                    "Sync med partner"
                },
                CommonMistakes = new List<string>
                {
                    "För liten step back",
                    "Glömmer weight transfer",
                    "Tappar frame",
                    "Fel timing"
                },
                Tags = new List<string> { "teknik", "anchor", "timing", "grund" }
            },
            ["compression-stretch"] = new PatternOrExercise
            {
                Id = "exercise-compression-stretch",
                Type = PatternType.Exercise,
                Name = "Compression & Stretch",
                Description = "Öva på att skapa och hantera compression och stretch - nyckelelement i WCS",
                Level = DanceLevel.Improver,
                DanceStyle = DanceStyle.WestCoastSwing,
                EstimatedMinutes = 20,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Starta i neutral position",
                    "Skapa compression genom att gå mot varandra",
                    "Skapa stretch genom att gå ifrån varandra",
                    "Öva push-pull dynamics"
                },
                TeachingPoints = new List<string>
                {
                    "Compression är inte push",
                    "Stretch maintainar connection",
                    "Both partners contribute",
                    "Smooth transitions"
                },
                CommonMistakes = new List<string>
                {
                    "För mycket kraft i compression",
                    "Tappar connection i stretch",
                    "Endast en partner arbetar",
                    "Abrupt transitions"
                },
                Tags = new List<string> { "teknik", "compression", "stretch", "dynamics" }
            },
            ["leading-following"] = new PatternOrExercise
            {
                Id = "exercise-leading-following",
                Type = PatternType.Exercise,
                Name = "Leading & Following",
                Description = "Utveckla förmågan att lead och follow tydligt och responsivt",
                Level = DanceLevel.Improver,
                DanceStyle = DanceStyle.WestCoastSwing,
                EstimatedMinutes = 20,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Leader: Öva tydliga leads med minimal kraft",
                    "Follower: Känn och respondera på leads",
                    "Byt roller för förståelse",
                    "Öva med blind dancing"
                },
                TeachingPoints = new List<string>
                {
                    "Lead med hela kroppen, inte bara händer",
                    "Follower väntar på lead, initierar inte",
                    "Clear intention i lead",
                    "Immediate response från follower"
                },
                CommonMistakes = new List<string>
                {
                    "För subtila leads",
                    "Follower anticiperar",
                    "Lead endast med armar",
                    "Delayed response"
                },
                Tags = new List<string> { "teknik", "leading", "following", "communication" }
            },
            ["timing-musicality"] = new PatternOrExercise
            {
                Id = "exercise-timing-musicality",
                Type = PatternType.Exercise,
                Name = "Timing & Musicality",
                Description = "Utveckla timing och förmåga att dansa till musiken, inte bara counts",
                Level = DanceLevel.Intermediate,
                DanceStyle = DanceStyle.WestCoastSwing,
                EstimatedMinutes = 20,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Lyssna på olika WCS tracks",
                    "Identifiera downbeat och &-counts",
                    "Öva basic till olika tempon",
                    "Experimentera med delays och accents"
                },
                BpmRangeMin = 80,
                BpmRangeMax = 120,
                TeachingPoints = new List<string>
                {
                    "WCS är dansat till musiken, inte över den",
                    "Find the groove",
                    "Anchor on strong beats",
                    "Experiment med syncopation"
                },
                CommonMistakes = new List<string>
                {
                    "Räknar endast counts",
                    "Ignorerar musiken",
                    "Fel tempo till track",
                    "Stiff musicality"
                },
                Tags = new List<string> { "teknik", "musicality", "timing", "rhythm" }
            },
            ["rotations-turns"] = new PatternOrExercise
            {
                Id = "exercise-rotations-turns",
                Type = PatternType.Exercise,
                Name = "Rotations & Turns",
                Description = "Teknikövning för att förbättra turns, spotting och balance under rotation",
                Level = DanceLevel.Intermediate,
                DanceStyle = DanceStyle.WestCoastSwing,
                EstimatedMinutes = 20,
                CreatedBy = creatorId,
                Steps = new List<string> 
                { 
                    "Öva spotting technique",
                    "Single, double, triple turns",
                    "Turn prep och exit",
                    "Balance drills"
                },
                TeachingPoints = new List<string>
                {
                    "Spotting förhindrar yrsel",
                    "Turn från core, inte shoulders",
                    "Maintain frame under rotation",
                    "Clean exits är lika viktiga som turns"
                },
                CommonMistakes = new List<string>
                {
                    "Ingen spotting",
                    "Turn med shoulders",
                    "Lost balance",
                    "Sloppy exits"
                },
                Tags = new List<string> { "teknik", "turns", "rotation", "balance" }
            }
        };
        
        var result = new Dictionary<string, string>();
        
        foreach (var kvp in exercises)
        {
            var existing = await context.Patterns.FirstOrDefaultAsync(p => p.Id == kvp.Value.Id);
            if (existing == null)
            {
                context.Patterns.Add(kvp.Value);
            }
            result[kvp.Key] = kvp.Value.Id;
        }
        
        return result;
    }
    
    private static async Task<List<string>> SeedLessonsAsync(
        DanceCourseDbContext context, 
        string creatorId, 
        Dictionary<string, string> patternIds,
        Dictionary<string, string> exerciseIds)
    {
        var lessons = new List<Lesson>
        {
            // Lektion 1: Grundsteg och introduktion till West Coast Swing
            new Lesson
            {
                Id = "lesson-wcs-01",
                Duration = 90,
                Notes = "Första lektionen fokuserar på grundläggande WCS-koncept och enkla rörelser",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Grundläggande uppvärmning och introduktion till WCS-musiken" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 20, ItemsJson = $"[\"{exerciseIds["frame-connection"]}\",\"{exerciseIds["anchor-step"]}\"]", Notes = "Fokus på frame och anchor step" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 40, ItemsJson = $"[\"{patternIds["starter-step"]}\",\"{patternIds["sugar-push"]}\"]", Notes = "Introduktion till starter step och sugar push" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Fri dans där eleverna får öva det de lärt sig till musik" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-60),
                UpdatedAt = DateTime.UtcNow.AddDays(-60)
            },
            
            // Lektion 2: Left Side Pass samt teknikövning Frame & Connection
            new Lesson
            {
                Id = "lesson-wcs-02",
                Duration = 90,
                Notes = "Introduktion till slot-baserade rörelser med Left Side Pass",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Repetition av sugar push som uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 20, ItemsJson = $"[\"{exerciseIds["frame-connection"]}\"]", Notes = "Fördjupning av frame och connection" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 40, ItemsJson = $"[\"{patternIds["left-side-pass"]}\"]", Notes = "Grundläggande Left Side Pass i slot" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Socialdans med sugar push och left side pass" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-53),
                UpdatedAt = DateTime.UtcNow.AddDays(-53)
            },
            
            // Lektion 3: Underarm Turn och Anchor Step
            new Lesson
            {
                Id = "lesson-wcs-03",
                Duration = 90,
                Notes = "Introduktion till rotationer med underarm turn",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Repetition av tidigare turer" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 20, ItemsJson = $"[\"{exerciseIds["anchor-step"]}\"]", Notes = "Förfining av anchor step technique" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 40, ItemsJson = $"[\"{patternIds["underarm-turn"]}\"]", Notes = "Underarm turn från left side pass" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Combination, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Kombinera left side pass och underarm turn" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-46),
                UpdatedAt = DateTime.UtcNow.AddDays(-46)
            },
            
            // Lektion 4: Whip och Compression & Stretch
            new Lesson
            {
                Id = "lesson-wcs-04",
                Duration = 90,
                Notes = "Introduktion till whip - en signature move i WCS",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Uppvärmning med tidigare turer" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 25, ItemsJson = $"[\"{exerciseIds["compression-stretch"]}\"]", Notes = "Viktigt för att förstå whip dynamics" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 40, ItemsJson = $"[\"{patternIds["whip"]}\"]", Notes = "Introduktion till whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 15, ItemsJson = "[]", Notes = "Öva whip i socialdans" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-39),
                UpdatedAt = DateTime.UtcNow.AddDays(-39)
            },
            
            // Lektion 5: Tuck Turn + Leading & Following
            new Lesson
            {
                Id = "lesson-wcs-05",
                Duration = 90,
                Notes = "Tuck turn för tighter rotationer",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Repetition av tidigare rörelser" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 25, ItemsJson = $"[\"{exerciseIds["leading-following"]}\"]", Notes = "Fokus på tydlig kommunikation i tuck turn" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 40, ItemsJson = $"[\"{patternIds["tuck-turn"]}\"]", Notes = "Tuck turn technique" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 15, ItemsJson = "[]", Notes = "Integration i socialdans" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-32),
                UpdatedAt = DateTime.UtcNow.AddDays(-32)
            },
            
            // Lektion 6: Sugar Push, Timing & Musicality
            new Lesson
            {
                Id = "lesson-wcs-06",
                Duration = 90,
                Notes = "Fördjupning av sugar push och musikalitet",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Musikalisk uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 30, ItemsJson = $"[\"{exerciseIds["timing-musicality"]}\"]", Notes = "Fokus på timing och musicality" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["sugar-push"]}\"]", Notes = "Advanced sugar push variations" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Dans till olika musikstilar" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                UpdatedAt = DateTime.UtcNow.AddDays(-25)
            },
            
            // Lektion 7: Whip with Inside Turn, Rotations & Turns
            new Lesson
            {
                Id = "lesson-wcs-07",
                Duration = 90,
                Notes = "Lägg till rotation till whip",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Repetition av whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 25, ItemsJson = $"[\"{exerciseIds["rotations-turns"]}\"]", Notes = "Rotation technique" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 40, ItemsJson = $"[\"{patternIds["whip-inside-turn"]}\"]", Notes = "Kombinera whip med inside turn" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 15, ItemsJson = "[]", Notes = "Öva i socialdans" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-18),
                UpdatedAt = DateTime.UtcNow.AddDays(-18)
            },
            
            // Lektion 8: Basket Whip, repetition och fri dans
            new Lesson
            {
                Id = "lesson-wcs-08",
                Duration = 90,
                Notes = "Avslutande lektion med mer avancerad move och repetition",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Allmän uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["basket-whip"]}\"]", Notes = "Introduktion till basket whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Repetition, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Repetition av alla turer från kursen" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 30, ItemsJson = "[]", Notes = "Fri dans - visa vad ni lärt er under kursen!" }
                }),
                TotalEstimatedMinutes = 90,
                CreatedAt = DateTime.UtcNow.AddDays(-11),
                UpdatedAt = DateTime.UtcNow.AddDays(-11)
            }
        };
        
        var lessonIds = new List<string>();
        
        foreach (var lesson in lessons)
        {
            var existing = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lesson.Id);
            if (existing == null)
            {
                // Store the sections JSON temporarily
                var sectionsJson = lesson.SectionsJson;
                // Set to empty array first to avoid EF tracking issues
                lesson.SectionsJson = "[]";
                context.Lessons.Add(lesson);
                await context.SaveChangesAsync();
                
                // Detach the lesson to avoid tracking issues
                context.Entry(lesson).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                
                // Use raw SQL to update SectionsJson to avoid EF tracking LessonSection entities
                await context.Database.ExecuteSqlRawAsync(
                    "UPDATE Lessons SET SectionsJson = {0} WHERE Id = {1}",
                    sectionsJson, lesson.Id);
            }
            lessonIds.Add(lesson.Id);
        }
        
        return lessonIds;
    }
    
    private static async Task SeedCourseAsync(
        DanceCourseDbContext context, 
        string creatorId, 
        List<string> lessonIds)
    {
        const string courseId = "course-wcs-ht25";
        var existing = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        
        if (existing == null)
        {
            var course = new Course
            {
                Id = courseId,
                Name = "West Coast Swing Grundkurs HT25",
                Level = DanceLevel.Beginner,
                DanceStyle = DanceStyle.WestCoastSwing,
                Type = CourseType.Weekly,
                DurationWeeks = 8,
                PlannedLessonCount = 8,
                Goals = new List<string>
                {
                    "Lära sig grundläggande West Coast Swing turer och teknik",
                    "Förstå slot-konceptet och WCS-estetiken",
                    "Utveckla frame, connection och musicality",
                    "Kunna dansa socialt med grundläggande turer"
                },
                ThemesByWeek = new List<string>
                {
                    "Vecka 1: Introduktion och grundsteg",
                    "Vecka 2: Slot och Left Side Pass",
                    "Vecka 3: Rotationer med Underarm Turn",
                    "Vecka 4: Whip - WCS signature move",
                    "Vecka 5: Tuck Turn",
                    "Vecka 6: Musicality och timing",
                    "Vecka 7: Advanced whip med rotation",
                    "Vecka 8: Basket Whip och kurssammanfattning"
                },
                LessonIds = lessonIds,
                CreatedBy = creatorId,
                CreatedAt = DateTime.UtcNow.AddDays(-65),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            };
            
            context.Courses.Add(course);
            
            // Update lessons to reference the course
            foreach (var lessonId in lessonIds)
            {
                var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
                if (lesson != null)
                {
                    lesson.CourseId = courseId;
                }
            }
        }
    }
    
    private static async Task<List<string>> SeedExtendedLessonsAsync(
        DanceCourseDbContext context, 
        string creatorId, 
        Dictionary<string, string> patternIds,
        Dictionary<string, string> exerciseIds)
    {
        // 12 lessons for the extended course (60 minutes each)
        var lessons = new List<Lesson>
        {
            // Lektion 1: Introduktion till West Coast Swing
            new Lesson
            {
                Id = "lesson-wcs-ext-01",
                Duration = 60,
                Notes = "Introduktion till WCS-dansen och dess historia",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Kort presentation och uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 15, ItemsJson = $"[\"{exerciseIds["frame-connection"]}\"]", Notes = "Grundläggande frame och connection" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["starter-step"]}\"]", Notes = "Introduktion till starter step" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Enkel socialdans till lågt tempo" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-90),
                UpdatedAt = DateTime.UtcNow.AddDays(-90)
            },
            
            // Lektion 2: Sugar Push - kompression och stretch
            new Lesson
            {
                Id = "lesson-wcs-ext-02",
                Duration = 60,
                Notes = "Fokus på Sugar Push och push-pull dynamics",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Repetition av starter step" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 15, ItemsJson = $"[\"{exerciseIds["anchor-step"]}\"]", Notes = "Fokus på anchor step" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["sugar-push"]}\"]", Notes = "Sugar Push grundligt" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans med starter step och sugar push" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-83),
                UpdatedAt = DateTime.UtcNow.AddDays(-83)
            },
            
            // Lektion 3: Left Side Pass
            new Lesson
            {
                Id = "lesson-wcs-ext-03",
                Duration = 60,
                Notes = "Introduktion till slot och Left Side Pass",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Uppvärmning med sugar push" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 10, ItemsJson = $"[\"{exerciseIds["frame-connection"]}\"]", Notes = "Frame under rörelse" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 35, ItemsJson = $"[\"{patternIds["left-side-pass"]}\"]", Notes = "Left Side Pass i slot" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-76),
                UpdatedAt = DateTime.UtcNow.AddDays(-76)
            },
            
            // Lektion 4: Underarm Turn
            new Lesson
            {
                Id = "lesson-wcs-ext-04",
                Duration = 60,
                Notes = "Första rotationen - Underarm Turn",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Repetition left side pass" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 10, ItemsJson = $"[\"{exerciseIds["anchor-step"]}\"]", Notes = "Anchor efter rotation" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 35, ItemsJson = $"[\"{patternIds["underarm-turn"]}\"]", Notes = "Underarm turn teknik" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans med alla turer hittills" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-69),
                UpdatedAt = DateTime.UtcNow.AddDays(-69)
            },
            
            // Lektion 5: Repetition och kombinationer
            new Lesson
            {
                Id = "lesson-wcs-ext-05",
                Duration = 60,
                Notes = "Repetition av grundturer och kombinationer",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Fri uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Repetition, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Repetition av alla grundturer" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Combination, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Kombinera turer i sekvenser" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 15, ItemsJson = "[]", Notes = "Längre socialdans" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-62),
                UpdatedAt = DateTime.UtcNow.AddDays(-62)
            },
            
            // Lektion 6: Compression & Stretch
            new Lesson
            {
                Id = "lesson-wcs-ext-06",
                Duration = 60,
                Notes = "Fördjupning i compression och stretch",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Dynamisk uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 25, ItemsJson = $"[\"{exerciseIds["compression-stretch"]}\"]", Notes = "Compression och stretch övningar" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 20, ItemsJson = $"[\"{patternIds["sugar-push"]}\"]", Notes = "Sugar push med bättre dynamics" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-55),
                UpdatedAt = DateTime.UtcNow.AddDays(-55)
            },
            
            // Lektion 7: Whip introduktion
            new Lesson
            {
                Id = "lesson-wcs-ext-07",
                Duration = 60,
                Notes = "Introduktion till Whip",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Uppvärmning med grundturer" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 15, ItemsJson = $"[\"{exerciseIds["leading-following"]}\"]", Notes = "Lead och follow för whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["whip"]}\"]", Notes = "Whip grundsteg" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Öva whip i socialdans" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-48),
                UpdatedAt = DateTime.UtcNow.AddDays(-48)
            },
            
            // Lektion 8: Tuck Turn
            new Lesson
            {
                Id = "lesson-wcs-ext-08",
                Duration = 60,
                Notes = "Tuck Turn - tight rotation",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Repetition av whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 15, ItemsJson = $"[\"{exerciseIds["rotations-turns"]}\"]", Notes = "Rotation teknik" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["tuck-turn"]}\"]", Notes = "Tuck turn" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-41),
                UpdatedAt = DateTime.UtcNow.AddDays(-41)
            },
            
            // Lektion 9: Musicality
            new Lesson
            {
                Id = "lesson-wcs-ext-09",
                Duration = 60,
                Notes = "Timing och musicality",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Musikalisk uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 30, ItemsJson = $"[\"{exerciseIds["timing-musicality"]}\"]", Notes = "Musicality övningar" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 25, ItemsJson = "[]", Notes = "Dansa till olika musikstilar" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-34),
                UpdatedAt = DateTime.UtcNow.AddDays(-34)
            },
            
            // Lektion 10: Whip med rotation
            new Lesson
            {
                Id = "lesson-wcs-ext-10",
                Duration = 60,
                Notes = "Whip with Inside Turn",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Repetition av whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Technique, AllocatedMinutes = 15, ItemsJson = $"[\"{exerciseIds["rotations-turns"]}\"]", Notes = "Rotation under whip" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 30, ItemsJson = $"[\"{patternIds["whip-inside-turn"]}\"]", Notes = "Whip med inside turn" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 10, ItemsJson = "[]", Notes = "Socialdans" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-27),
                UpdatedAt = DateTime.UtcNow.AddDays(-27)
            },
            
            // Lektion 11: Basket Whip
            new Lesson
            {
                Id = "lesson-wcs-ext-11",
                Duration = 60,
                Notes = "Basket Whip introduktion",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Uppvärmning" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Patterns, AllocatedMinutes = 35, ItemsJson = $"[\"{patternIds["basket-whip"]}\"]", Notes = "Basket whip steg för steg" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Socialdans med alla turer" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = DateTime.UtcNow.AddDays(-20)
            },
            
            // Lektion 12: Kursavslutning och sammanfattning
            new Lesson
            {
                Id = "lesson-wcs-ext-12",
                Duration = 60,
                Notes = "Kursavslutning - repetition och fri dans",
                CreatedBy = creatorId,
                SectionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Warmup, AllocatedMinutes = 5, ItemsJson = "[]", Notes = "Sista uppvärmningen" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Repetition, AllocatedMinutes = 20, ItemsJson = "[]", Notes = "Repetition av alla turer från kursen" },
                    new { Id = Guid.NewGuid().ToString(), Type = (int)LessonSectionType.Social, AllocatedMinutes = 35, ItemsJson = "[]", Notes = "Avslutande socialdans - visa vad ni lärt er!" }
                }),
                TotalEstimatedMinutes = 60,
                CreatedAt = DateTime.UtcNow.AddDays(-13),
                UpdatedAt = DateTime.UtcNow.AddDays(-13)
            }
        };
        
        var lessonIds = new List<string>();
        
        foreach (var lesson in lessons)
        {
            var existing = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lesson.Id);
            if (existing == null)
            {
                // Store the sections JSON temporarily
                var sectionsJson = lesson.SectionsJson;
                // Set to empty array first to avoid EF tracking issues
                lesson.SectionsJson = "[]";
                context.Lessons.Add(lesson);
                await context.SaveChangesAsync();
                
                // Detach the lesson to avoid tracking issues
                context.Entry(lesson).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                
                // Use raw SQL to update SectionsJson to avoid EF tracking LessonSection entities
                await context.Database.ExecuteSqlRawAsync(
                    "UPDATE Lessons SET SectionsJson = {0} WHERE Id = {1}",
                    sectionsJson, lesson.Id);
            }
            lessonIds.Add(lesson.Id);
        }
        
        return lessonIds;
    }
    
    private static async Task SeedExtendedCourseAsync(
        DanceCourseDbContext context, 
        string creatorId, 
        List<string> lessonIds)
    {
        const string courseId = "course-wcs-vt26";
        var existing = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        
        if (existing == null)
        {
            var course = new Course
            {
                Id = courseId,
                Name = "West Coast Swing Fortsättningskurs VT26",
                Level = DanceLevel.Improver,
                DanceStyle = DanceStyle.WestCoastSwing,
                Type = CourseType.Weekly,
                DurationWeeks = 12,
                PlannedLessonCount = 12,
                Goals = new List<string>
                {
                    "Fördjupa kunskapen i West Coast Swing turer och teknik",
                    "Utveckla bättre musicality och timing",
                    "Lära sig mer avancerade variationer av whip",
                    "Kunna dansa flytande socialt med varierad repertoar"
                },
                ThemesByWeek = new List<string>
                {
                    "Vecka 1: Introduktion och grundläggande repetition",
                    "Vecka 2: Sugar Push - kompression och stretch",
                    "Vecka 3: Left Side Pass fördjupning",
                    "Vecka 4: Underarm Turn variationer",
                    "Vecka 5: Repetition och kombinationer",
                    "Vecka 6: Compression & Stretch fördjupning",
                    "Vecka 7: Whip introduktion",
                    "Vecka 8: Tuck Turn",
                    "Vecka 9: Musicality och timing",
                    "Vecka 10: Whip med inside turn",
                    "Vecka 11: Basket Whip",
                    "Vecka 12: Kursavslutning och fri dans"
                },
                LessonIds = lessonIds,
                CreatedBy = creatorId,
                CreatedAt = DateTime.UtcNow.AddDays(-95),
                UpdatedAt = DateTime.UtcNow.AddDays(-12)
            };
            
            context.Courses.Add(course);
            
            // Update lessons to reference the course
            foreach (var lessonId in lessonIds)
            {
                var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
                if (lesson != null)
                {
                    lesson.CourseId = courseId;
                }
            }
        }
    }
}
