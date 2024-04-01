import axios from 'axios';

class FeedService {

    async getUpworkRssFeedDetails(id: String, feedId: String): Promise<UpworkRssFeedDetails> {
        try {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/persons/' + id + '/upwork-rss-feeds/' + feedId);
            console.log('Feed details:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }

    async addNewUpworkRssFeed(id: String,
        data: UpworkRssFeedDetails): Promise<void> {

        const dataToSend =
        {
            absoluteFeedUrl: data.absoluteFeedUrl,
            title: data.title,
            minimumFetchIntervalInMinutes: data.minimumFetchIntervalInMinutes || 0,
            autoSendEmailEnabled: data.autoSendEmailEnabled,
            autoGenerateProposalsEnabled: data.autoGenerateProposalsEnabled,
            preferredProfileId: data.preferredProfileId
        }

        try {
            await axios.post(import.meta.env.VITE_API_URL + '/persons/' + id + '/upwork-rss-feeds/', dataToSend);
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }

    async updateUpworkRssFeedDetails(id: String,
        data: UpworkRssFeedDetails): Promise<void> {

        const dataToSend =
        {
            absoluteFeedUrl: data.absoluteFeedUrl,
            title: data.title,
            minimumFetchIntervalInMinutes: data.minimumFetchIntervalInMinutes || 0,
            autoSendEmailEnabled: data.autoSendEmailEnabled,
            autoGenerateProposalsEnabled: data.autoGenerateProposalsEnabled,
            preferredProfileId: data.preferredProfileId
        }

        try {
            await axios.put(import.meta.env.VITE_API_URL + '/persons/' + id + '/upwork-rss-feeds/' + data.id, dataToSend);
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }
}

export interface UpworkRssFeedDetails {
    id: String,
    absoluteFeedUrl: String,
    rootUrl: String,
    relativeUrl: String,
    title: String,
    minimumFetchIntervalInMinutes: number,
    lastFetchTimeUtc: String,
    autoSendEmailEnabled: boolean,
    autoGenerateProposalsEnabled: boolean,
    preferredProfileId: number | null,
    createdUtc: String
}

export default new FeedService();
