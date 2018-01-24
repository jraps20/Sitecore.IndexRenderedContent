# Sitecore.IndexRenderedContent

Easily and strategically index modular content in Sitecore. Patch config determines which renderings and which datasources associated with a page item should be indexed. All results stored in the standard "_content" field in the index.

### Support

Sitecore 9+ - _. If you use this package with success, please let me know and I will update supported versions. To the best of my knowledge, the depedent logic has not been updated._

Sitecore 8.1 and newer - Verified on 8.1 up to 8.2u5. 

Sitecore 8.0 and lower - This package will not work out of the box on these versions due to the LanguageFallbackManager class not existing. If LanguageFallback is not required, remove the reference from ExtractRenderingDatasourceItems.cs.

# Installation

1. Install the NuGet package into your Sitecore web project: (link to nuget)
2. Review `App_Config\Include\Z.IndexRenderedContent.config`
3. Update `<defaultLuceneIndexConfiguration>` to match the index configuration you're targeting. If using default Lucene configuration, no change necessary.
4. Add indexable renderings to the `<indexableRenderings>` element following guidelines. These are the renderings that will be indexed. Without adding renderings here, nothing will be indexed.
5. Build project
6. Rebuild depedendent search indexes

# Configuration

This project is intended to be highly configurable. It makes certain assumptions in the base package, however everything that matters can be configured. All configurable items have an associated pipeline that can be modified.

If there is anything outlined below that does not meet your requirements, then patch in your own custom processor.

# Pipelines

## <indexing.getDependencies>

This is a default Sitecore pipeline. It is used to determine what items to update if a dependent item is updated. 

In this package, the included processor, on item save, looks for any items that refer to this item from the Layout or FinalLayout fields. If a match is found, this implies a "Page" item in Sitecore has a rendering with this item set as a datasource. This triggers the index to add the associated "Page" item to the index.

***It is worth being extra clear that this processor will only find items that are directly linked as a datasource. It will not find items, for example, that may be a child of a datasource item. If you had a "Tabbed" rendering that points to a primary datasource, and then it relied on child items for each "Tab". Updating the individual tabs would not trigger the primary page item to be reindexed.***

## <indexing.getIndexableRenderings>

This is a custom pipeline that is used to determine which renderings to index. This is to avoid indexing rendergs such as Navigation, Related Articles or Calls to Action. These types of renderings are used on many pages and are not relevant or specific to the page in question.

By default, it reads the `<indexableRenderings>` section of the config and only allows renderings defined here to have their datasources indexed. If you require unique conditions, add a new processor as needed.

## <indexing.extractRenderingsDatasources>

This is a custom pipeline responsible for extracting valid renderings and their datasources from the indexed item. The two included processors review the Layout and FinalLayout fields for all valid renderings that contain a datasource. 

## <indexing.getDatasourceContent>

This is a custom pipeline used to extract the data from a datasource. By default, it iterates all fields on a datasource item and only indexes fields that are Text Fields. It uses `Sitecore.ContentSearch.IndexOperationsHelper.IsTextField` to make this classification. It also automatically excludes all fields that begin with "__" as these are system fields.

It also strips HTML tags from the content for a field such as a rich text field. This is to ensure the index contains clean, ready-to-search content.

## <indexing.renderedContent.Saving>

This is a custom pipeline that is executed directly before the extracted content is stored in the index. By default, the only processor here simply formats the content slightly by removing extra whitespace contained in the field.
