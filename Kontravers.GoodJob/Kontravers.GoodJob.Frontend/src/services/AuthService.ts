import axios from 'axios';

class AuthService {
    async getKento(): Promise<void> {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        const code = urlParams.get('code');
        try {
            const response = await axios.post('https://goodjob-auth.kontrave.rs/connect/token', {
                grant_type: "authorization_code",
                code: code,
                redirect_uri: 'http://localhost:5173/callback'
            });
            console.log(response);
        } catch (error) {
            console.error('Error logging in:', error);
            throw error;
        }
    }
}

export default new AuthService();